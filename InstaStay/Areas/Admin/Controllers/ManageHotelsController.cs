using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;

namespace InstaStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManageHotels : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public ManageHotels(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult ShowAllHotels()
        {
            var Hotels = unitOfWork.hotelRepository.Get(includeprops: e => e.Include(e => e.Rooms).Include(e => e.HotelManager));
            return View(Hotels);
        }
        public IActionResult Delete(int id)
        {
            var hotel = unitOfWork.hotelRepository.GetOne(e => e.Id == id);
            if (id != null)
            {
                unitOfWork.hotelRepository.Delete(hotel);
                unitOfWork.Commit();
                TempData["success"] = $"Hotel Deleted Successfully It's Name {hotel.Name}";
                return RedirectToAction("ShowAllHotels");
            }
            TempData["success"] = $"Can't delete this hotel";
            return RedirectToAction("ShowAllHotels");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var hotel = unitOfWork.hotelRepository.GetOne(e => e.Id == id);
            return View(hotel);
        }
        [HttpPost]
        public IActionResult Edit(Hotel hotel, IFormFile? CoverImage)
        {
            var existingHotel = unitOfWork.hotelRepository.GetOne(e => e.Id == hotel.Id);

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
                return RedirectToAction("ShowAllHotels");
            }

            return RedirectToAction("ShowAllHotels");
        }
        public IActionResult Details(int id)
        {
        var Hotel = unitOfWork.hotelRepository.GetOne(e => e.Id == id);
        return View(Hotel);
        }
    }
}
