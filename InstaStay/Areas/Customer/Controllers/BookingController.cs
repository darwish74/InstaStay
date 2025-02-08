using DataAccess;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;
using Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace InstaStay.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BookingController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        public BookingController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public IActionResult Book(int id)
        {
            var room = unitOfWork.roomRepository.GetOne(e => e.Id == id);
            if (room == null || !room.Availbility)
            {
                TempData["success"] = "This room is currently unavailable.";
                return RedirectToAction("Index", "Home"); 
            }

            var model = new BookingVM
            {
                RoomId = room.Id,
                RoomType = room.RoomType,
                PricePerNight = room.PricePerNight,
                HotelId = room.HotelId,
                CheckINDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(1),
                IsAvailable = true
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult CheckRoomAvailability(int roomId, string checkInDate, string checkOutDate)
        {
            if (DateTime.TryParse(checkInDate, out DateTime inDate) && DateTime.TryParse(checkOutDate, out DateTime outDate))
            {
                bool isAvailable = IsRoomAvailable(roomId, inDate, outDate);
                return Json(new { available = isAvailable });
            }
            return Json(new { available = false, error = "Invalid date format" });
        }
        private bool IsRoomAvailable(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var existingBookings = unitOfWork.BookingRepository.Get()
                .Where(b => b.RoomId == roomId && (b.CheckINDate < checkOutDate && b.CheckOutDate > checkInDate))
                .ToList();

            return !existingBookings.Any();
        }
        [HttpGet]
        public IActionResult ValidateCoupon(string couponCode)
        {
            var coupon = unitOfWork.CouponRepository.GetOne(c => c.Code == couponCode && c.IsActive);
            if (coupon != null && DateTime.Now >= coupon.ValidFrom && DateTime.Now <= coupon.ValidTo)
            {
                return Json(new { valid = true, discount = coupon.DiscountValue });
            }
            return Json(new { valid = false });
        }
    [HttpPost]
    public async Task<IActionResult> CreateBooking(BookingVM model)
    {
    var user = await userManager.GetUserAsync(User);
    if (user == null)
    {
        return RedirectToAction("Login", "Account", new { area = "Identity" });
    }

    var room = unitOfWork.roomRepository.GetOne(e => e.Id == model.RoomId);
    if (room == null || !room.Availbility)
    {
        ModelState.AddModelError("", "Sorry, this room is no longer available.");
        model.IsAvailable = false;
        return View("Book", model);
    }

    if (!IsRoomAvailable(model.RoomId, model.CheckINDate, model.CheckOutDate))
    {
        ModelState.AddModelError("", "Sorry, this room is already booked for the selected dates.");
        model.IsAvailable = false;
        return View("Book", model);
    }

    decimal totalAmount = (decimal)(room.PricePerNight * (model.CheckOutDate - model.CheckINDate).Days);
    
    if (!string.IsNullOrEmpty(model.CouponCode))
    {
        var coupon = unitOfWork.CouponRepository.GetOne(c => c.Code == model.CouponCode && c.IsActive);
        if (coupon != null && DateTime.Now >= coupon.ValidFrom && DateTime.Now <= coupon.ValidTo)
        {
            if (coupon.DiscountType == "Percentage")
            {
                totalAmount -= totalAmount * (coupon.DiscountValue / 100);
            }
            else
            {
                totalAmount -= coupon.DiscountValue;
            }
        }
         else
          {
            ModelState.AddModelError("", "Invalid or expired coupon.");
            return View("Book", model);
          }
          }
          var booking = new Booking
          {
           UserId = user.Id,
           HotelId = room.HotelId,
           RoomId = model.RoomId,
           CheckINDate = model.CheckINDate,
           CheckOutDate = model.CheckOutDate,
           BookingStatus = "Pending",
           TotalAmount = (int)totalAmount
         };

          unitOfWork.BookingRepository.Create(booking);
          unitOfWork.Commit();
          TempData["success"] = "Room booked successfully";
          return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> MyBookings()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var bookings = unitOfWork.BookingRepository.Get(b => b.UserId == user.Id,includeprops:e=>e.Include(e=>e.Hotel).Include(e=>e.Room)).ToList();
            return View(bookings);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = unitOfWork.BookingRepository.GetOne(b => b.Id == id && b.UserId == user.Id);

            if (booking == null)
            {
                return NotFound();
            }
            unitOfWork.BookingRepository.Delete(booking);
            unitOfWork.Commit();
            TempData["success"] = "Book Deleted Successfully";
            return RedirectToAction("MyBookings");
        }

    }
}
