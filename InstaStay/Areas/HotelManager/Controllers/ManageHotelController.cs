using Azure.Core;
using DataAccess;
using DataAccess.Repositories;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;
namespace InstaStay.Areas.hotelManager.Controllers
{
    [Area("hotelManager")]
    [Authorize(Roles = "Hotel Manager")]
    public class ManageHotelController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork unitOfWork;

        public ManageHotelController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpGet]
        public IActionResult NewHotelRequest()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewHotelRequest(NewHotelRequests request,IFormFile? CoverImage,string UserName)
        {
            if (ModelState.IsValid)
            {
                if (CoverImage != null && CoverImage.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(CoverImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\HotelImages", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        CoverImage.CopyTo(stream);
                    }
                    request.CoverImage = fileName;
                }
                else
                {
                    ModelState.AddModelError("Image", "Please upload an image.");
                    return View(request);
                }
                request.HotelManager=UserName;
                unitOfWork.NewHotelRequestsRepository.Create(request);
                unitOfWork.Commit();
                TempData["success"] = "Request Created Successfully";
                return RedirectToAction("Dashboard");
            }
          return View(request);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Hotel =unitOfWork.hotelRepository.GetOne(filter: e => e.Id == id,includeprops:e=>e.Include(e=>e.HotelImages).Include(e=>e.Amentities));
            return View(Hotel);
        }     
        [HttpGet]
        public IActionResult ShowAllHotels(string UserName)
        {
            var Hotels = unitOfWork.hotelRepository.Get(e=>e.HotelManager.Name==UserName).ToList();
            return View(Hotels);
        }
        [HttpGet]
        public IActionResult AddRoom(int HotelId)
        {
        ViewBag.Id= HotelId; 
        return View();
        }
        [HttpPost]
        public IActionResult AddRoom(Room room,int HotelId) 
        {
            var hotel = unitOfWork.hotelRepository.GetOne(e => e.Id == HotelId);
            if (hotel != null)
            {
               room.HotelId = HotelId;  
               unitOfWork.roomRepository.Create(room);
               unitOfWork.Commit();
               TempData["success"] = $"Room Added Successfully to Hotel {hotel.Name}";
               return RedirectToAction("Dashboard");
            }
        return View(room);  
        }
        [HttpPost]
        public IActionResult Edit(Hotel hotel, IFormFile? CoverImage)
        {
            var existingHotel = unitOfWork.hotelRepository.GetOne(e => e.Id == hotel.Id,includeprops:e=>e.Include(e=>e.HotelManager));
            if (existingHotel == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (CoverImage != null && CoverImage.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(CoverImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\HotelImages", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        CoverImage.CopyTo(stream);
                    }
                    if (existingHotel.CoverImage != null)
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\HotelImages", existingHotel.CoverImage);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    existingHotel.CoverImage = fileName;
                }
                existingHotel.Name = hotel.Name;
                existingHotel.Address = hotel.Address;
                existingHotel.Description = hotel.Description;
                existingHotel.ContactInfo = hotel.ContactInfo;
                existingHotel.Stars = hotel.Stars;
                unitOfWork.hotelRepository.Alter(existingHotel);
                unitOfWork.Commit();
                TempData["success"] = "Hotel Edited Successfully";
                return RedirectToAction("ShowAllHotels", new {UserName = existingHotel.HotelManager.Name});
            }
            return RedirectToAction("ShowAllHotels");
        }
        public IActionResult Delete(int id)
        {
            var hotel = unitOfWork.hotelRepository.GetOne(e => e.Id == id, includeprops: e => e.Include(e => e.HotelManager));
            if (id != null)
            {
                unitOfWork.hotelRepository.Delete(hotel);
                unitOfWork.Commit();
                TempData["success"] = "Hotel and all its rooms deleted successfully!";
                return RedirectToAction("ShowAllHotels", new { UserName = hotel.HotelManager.Name });
            }
            TempData["success"] = $"Can't delete this hotel";
            return RedirectToAction("ShowAllHotels", new { UserName = hotel.HotelManager.Name });
        }
        public IActionResult Details(int id)
        {
            var Hotel = unitOfWork.hotelRepository.GetOne(e => e.Id == id);
            return View(Hotel);
        }
        public IActionResult AddHotelImages(int id,IFormFile hotelImages)
        {
          var hotel= unitOfWork.hotelRepository.GetOne(e => e.Id == id, includeprops: e => e.Include(e => e.HotelManager));
            if (hotel != null)
            {
               var NewHotelImage = new HotelImages();
               if(hotelImages != null && hotelImages.Length > 0)
               {
                    var fileName= Guid.NewGuid().ToString() + Path.GetExtension(hotelImages.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\HIL", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        hotelImages.CopyTo(stream);
                    }
                    NewHotelImage.ImageURL= fileName;
                    NewHotelImage.HotelId = id; 
                    unitOfWork.HotelImagesRepository.Create(NewHotelImage); 
                    unitOfWork.Commit();
                    TempData["success"] = "Hotel Image added successfully";
                    return RedirectToAction("ShowAllHotels", new { UserName = hotel.HotelManager.Name });
                }
            }
            TempData["success"] = "No Images added";
            return RedirectToAction("ShowAllHotels", new { UserName = hotel.HotelManager.Name });
        }
    }
}
