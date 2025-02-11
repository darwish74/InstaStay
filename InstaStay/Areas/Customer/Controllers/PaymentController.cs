using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;
using Models.Utilities;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace InstaStay.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PaymentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailSender emailSender;

        public PaymentController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork,IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.emailSender = emailSender;
        }
        public async Task<IActionResult> PaySingleBooking(int id)
        {
            var userId = userManager.GetUserId(User);
            var booking = unitOfWork.BookingRepository
                .GetOne(b => b.Id == id && b.UserId == userId && b.BookingStatus == "Pending",
                        includeprops: q => q.Include(b => b.Room));

            if (booking == null)
            {
                TempData["error"] = "Invalid booking selection.";
                return RedirectToAction("MyBookings", "Booking");
            }
            return await ProcessPayment(new List<Booking> { booking });
        }
        public async Task<IActionResult> PayAllBookings()
        {
            var userId = userManager.GetUserId(User);
            var bookings = unitOfWork.BookingRepository
                .Get(b => b.UserId == userId && b.BookingStatus == "Pending")
                .Include(b => b.Room)
                .ToList();

            if (!bookings.Any())
            {
                TempData["error"] = "No pending bookings available for payment.";
                return RedirectToAction("MyBookings", "Booking");
            }
            return await ProcessPayment(bookings); 
        }
        private async Task<IActionResult> ProcessPayment(List<Booking> bookings)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = Url.Action("PaymentSuccess", "Payment",
                    new { area = "Customer", bookingId = bookings.First().Id }, Request.Scheme),
                CancelUrl = Url.Action("PaymentCancel", "Payment", new { area = "Customer" }, Request.Scheme),
            };

            foreach (var item in bookings)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"{item.Room.RoomType} - {item.Room.Hotel?.Name ?? "Hotel"}",
                            Description = $"Check-in: {item.CheckINDate:dd MMM yyyy}, " +
                                          $"Check-out: {item.CheckOutDate:dd MMM yyyy}"
                        },
                        UnitAmount = (long)(item.TotalAmount * 100),
                    },
                    Quantity = 1,
                });
            }

            try
            {
                var service = new SessionService();
                var session = await service.CreateAsync(options);
                return Redirect(session.Url);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Payment processing error: " + ex.Message;
                return RedirectToAction("MyBookings", "Booking");
            }
        }
        public async Task<IActionResult> PaymentSuccess(int bookingId)
        {
            var userId = userManager.GetUserId(User);
            var user = await userManager.FindByIdAsync(userId);

            var booking = unitOfWork.BookingRepository
                .GetOne(b => b.Id == bookingId && b.UserId == userId && b.BookingStatus == "Pending",
                        includeprops: q => q.Include(b => b.Room).ThenInclude(r => r.Hotel));

            if (booking == null)
            {
                TempData["error"] = "Invalid booking selection.";
                return RedirectToAction("MyBookings", "Booking");
            }
            booking.BookingStatus = "Confirmed";
            unitOfWork.Commit();
            string subject = "Booking Confirmation - InstaStay";
            string message = $@"
            <h2>Booking Confirmation</h2>
            <p>Dear {user.FirstName},</p>
            <p>Thank you for your payment. Your booking has been confirmed.</p>
            <h3>Booking Details:</h3>
            <ul>
            <li><strong>Hotel:</strong> {booking.Room.Hotel?.Name ?? "Hotel"}</li>
            <li><strong>Room:</strong> {booking.Room.RoomType}</li>
            <li><strong>Check-in:</strong> {booking.CheckINDate:dd MMM yyyy}</li>
            <li><strong>Check-out:</strong> {booking.CheckOutDate:dd MMM yyyy}</li>
            <li><strong>Total Amount:</strong> ${booking.TotalAmount:F2}</li>
        </ul>
        <p>We look forward to welcoming you!</p>
        <p><strong>InstaStay Team</strong></p>";

            await emailSender.SendEmailAsync(user.Email, subject, message);

            TempData["success"] = "Payment successful! Your booking is now confirmed.";

            return RedirectToAction("MyBookings", "Booking");
        }
        public IActionResult PaymentCancel()
        {
            TempData["error"] = "Payment was canceled. Please try again.";
            return RedirectToAction("MyBookings", "Booking");
        }
    }
}
