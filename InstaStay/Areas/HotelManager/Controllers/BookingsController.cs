using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;
using Models.ViewModels;

namespace InstaStay.Areas.HotelManager.Controllers
{
    [Area("hotelManager")]
    [Authorize]
    
    public class BookingsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public BookingsController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index(string UserName)
        {
            var hotels =unitOfWork.hotelRepository.Get(e=>e.HotelManager.Name==UserName).ToList();
            
            return View(hotels);
        }

        public IActionResult AllBooking(int HotelId)
        {
            var Booking=unitOfWork.BookingRepository.Get(filter:e=>e.Hotel.Id==HotelId, includeprops: e=>e.Include(e=>e.User).Include(e=>e.Room)).ToList();
            if (Booking == null || !Booking.Any())
            {
                return View(new List<Models.Models.Booking>()); // Return empty list instead of null
            }

            return View(Booking);
        }
    }
}
