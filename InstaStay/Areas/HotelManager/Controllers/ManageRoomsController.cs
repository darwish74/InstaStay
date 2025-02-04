using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
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
            var room=unitOfWork.roomRepository.GetOne(e=>e.Id==id,includeprops:e=>e.Include(e=>e.RoomImages));
            ViewBag.Id=room.Id; 
            return View(room.RoomImages ?? new List<RoomImages>());
        }
        [HttpGet]
        public IActionResult AddRoomImage(int id)
        {
            var room = unitOfWork.roomRepository.GetOne(e => e.Id == id);
            ViewBag.Id = room.Id;
            return View();  
        }
        [HttpPost]
        public IActionResult AddRoomImage(int id, IFormFile RoomImage)
        {
                var room = unitOfWork.roomRepository.GetOne(e => e.Id == id);
                if (room != null)
                {
                    var NewRoomImage = new RoomImages();
                    if (RoomImage != null && RoomImage.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(RoomImage.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\RoomImages", fileName);
                        using (var stream = System.IO.File.Create(filePath))
                        {
                          RoomImage.CopyTo(stream);
                        }
                        NewRoomImage.Image = fileName;
                        NewRoomImage.RoomId = id;
                        unitOfWork.RoomImagesRepository.Create(NewRoomImage);
                        unitOfWork.Commit();
                        TempData["success"] = "Room Image added successfully";
                        return RedirectToAction("ShowAllRoomImages", new {id= room.Id});
                    }
                }
                TempData["success"] = "No Images added";
                return RedirectToAction("ShowAllRoomImages", new { id = room.Id });
        }
            
    }    
}
