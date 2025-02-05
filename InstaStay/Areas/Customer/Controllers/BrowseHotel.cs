using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;

namespace InstaStay.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BrowseHotel : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public BrowseHotel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
          public IActionResult HotelDetails(int hotelId)
        {
            var hotel= unitOfWork.hotelRepository.GetOne(e=>e.Id== hotelId, includeprops:e=>e.Include(e=>e.Amentities).
                                                                                       Include(e=>e.HotelManager).
                                                                                       Include(e=>e.Promotions).
                                                                                       Include(e=>e.HotelImages).
                                                                                       Include(e => e.Rooms).
                                                                                       Include(e=>e.Reviews).ThenInclude(e=>e.User));

            return View(hotel);
        }
        public IActionResult RoomDetails(int id)
        {
            var room = unitOfWork.roomRepository.GetOne(
                e => e.Id == id,
                includeprops: e => e.Include(e => e.RoomImages)
            );
            if (room == null)
            {
                return NotFound();
            }
            room.RoomImages = room.RoomImages ?? new List<RoomImages>();

            return View(room);
        }

    }
}
