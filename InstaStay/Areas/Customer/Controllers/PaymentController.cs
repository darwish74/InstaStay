using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;
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

        public PaymentController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Pay(int id)
        {
            var userId = userManager.GetUserId(User);
            var bookings = new List<Booking>();

            if (id != 0)
            {
                var singleBooking = unitOfWork.BookingRepository
                    .GetOne(b => b.Id == id && b.UserId == userId && b.BookingStatus == "Pending",
                            includeprops: q => q.Include(b => b.Room));

                if (singleBooking == null)
                {
                    TempData["error"] = "Invalid booking selection.";
                    return RedirectToAction("MyBookings", "Booking");
                }
                bookings.Add(singleBooking);
            }
            else
            {
                bookings = unitOfWork.BookingRepository
                    .Get(b => b.UserId == userId && b.BookingStatus == "Pending")
                    .Include(b => b.Room)
                    .ToList();
            }
            if (!bookings.Any())
            {
                TempData["error"] = "No pending bookings available for payment.";
                return RedirectToAction("MyBookings", "Booking");
            }
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = Url.Action("PaymentSuccess", "Payment", new { area = "Customer" }, Request.Scheme),
                CancelUrl = Url.Action("PaymentCancel", "Payment", new { area = "Customer" }, Request.Scheme),
            };
            foreach (var item in bookings)
            {
                if (item.Room == null)
                {
                    TempData["error"] = $"Booking ID {item.Id} has no associated room.";
                    return RedirectToAction("MyBookings", "Booking");
                }
                if (item.TotalAmount == null)
                {
                    TempData["error"] = $"Booking ID {item.Id} has an invalid total amount.";
                    return RedirectToAction("MyBookings", "Booking");
                }

                Console.WriteLine($"Booking ID: {item.Id}, Room: {item.Room.RoomType}, Total Amount: {item.TotalAmount}");

                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Room.RoomType ?? "Unknown Room"
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
        public IActionResult PaymentSuccess()
        {
            var userId = userManager.GetUserId(User);
            var bookings = unitOfWork.BookingRepository
                .Get(b => b.UserId == userId && b.BookingStatus == "Pending")
                .Include(b => b.Room) 
                .ToList();

            foreach (var booking in bookings)
            {
                booking.BookingStatus = "Confirmed";
            }
            unitOfWork.Commit();

            TempData["success"] = "Payment successful! Your bookings are confirmed.";
            return RedirectToAction("MyBookings", "Booking");
        }
        public IActionResult PaymentCancel()
        {
            TempData["error"] = "Payment was canceled. Please try again.";
            return RedirectToAction("MyBookings", "Booking");
        }
    }
}
