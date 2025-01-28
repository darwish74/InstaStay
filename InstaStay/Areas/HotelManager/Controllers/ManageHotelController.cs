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
            var Hotel =unitOfWork.hotelRepository.GetOne(filter: e => e.Id == id);
            return View(Hotel);
        }
        [HttpPost]
        public IActionResult Edit(Hotel hotel, IFormFile? CoverImage)
        {
            ModelState.Remove("CoverImage");
            var oldHotel=unitOfWork.hotelRepository.GetOne(filter:e=>e.Id == hotel.Id);
            if (ModelState.IsValid)
            {
                if (CoverImage != null && CoverImage.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(CoverImage.FileName);

                    // Save in wwwroot
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\HotelImages", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        CoverImage.CopyTo(stream);
                    }

                    // Delete old img
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\HotelImages", oldHotel.CoverImage);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    hotel.CoverImage = fileName;
                }
                else
                {
                    hotel.CoverImage = oldHotel.CoverImage;
                }
                unitOfWork.hotelRepository.Alter(hotel);
                unitOfWork.Commit();
                TempData["success"] = "Edit Hotel Successfully";
                return RedirectToAction("ShowAllHotels");


            }
            return RedirectToAction("ShowAllHotels");   
        }





        [HttpGet]
        public IActionResult ShowAllHotels(string UserName)
        {
            var Hotels = unitOfWork.hotelRepository.Get(e=>e.HotelManager.Name==UserName).ToList();
            return View(Hotels);
        }
        [HttpGet]
        public IActionResult AddRoom(int Id)
        {
        return View();
        }
        [HttpPost]
        public IActionResult AddRoom(Room room) 
        {
        return View(room);  
        }






    }
}
