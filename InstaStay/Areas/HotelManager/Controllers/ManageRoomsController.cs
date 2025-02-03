using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Models.IRepositories;
using Models.Models;

namespace InstaStay.Areas.HotelManager.Controllers
{
    [Area("hotelManager")]
    [Authorize(Roles = "Hotel Manager")]
    public class ManageRoomsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ManageRoomsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var room = unitOfWork.roomRepository.GetOne(e=>e.Id==id);    
            return View(room);  
        }
        [HttpPost]
        public IActionResult Edit(Room room) 
        {
             if(ModelState.IsValid) { 
             var ExistingRoom = unitOfWork.roomRepository.GetOne(e => e.Id == room.Id);
             if(ExistingRoom != null)
             {
               ExistingRoom.PricePerNight = room.PricePerNight; 
               ExistingRoom.Availbility = room.Availbility; 
               ExistingRoom.BedType = room.BedType;
               ExistingRoom.BedType = room.BedType;
               ExistingRoom.Capacity = room.Capacity;
               unitOfWork.roomRepository.Alter(ExistingRoom);
               unitOfWork.Commit();
               TempData["success"] = "Room Updated Successfully";
               return RedirectToAction("Edit", "ManageHotel", new {id=ExistingRoom.HotelId}); 
             }
             }
            return View(room);
        }
        public IActionResult Delete(int id)
        {
            var room=unitOfWork.roomRepository.GetOne(room=>room.Id==id);
            if(room != null) 
            {
                unitOfWork.roomRepository.Delete(room);
                unitOfWork.Commit();
                TempData["success"] = "Room Deleted Successfully";
                return RedirectToAction("Edit", "ManageHotel", new { id = room.HotelId });
            }
            TempData["success"] = "No Updates";
            return RedirectToAction("Edit", "ManageHotel", new { id = room.HotelId });
        }
        public IActionResult ShowAllRoomImages(int id)
        { 
            var roomImages=unitOfWork.RoomImagesRepository.GetOne(e=>e.RoomId==id); 
            return View(roomImages);    
        }
    }
}
