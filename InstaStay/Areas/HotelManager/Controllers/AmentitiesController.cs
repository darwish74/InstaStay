using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;

namespace InstaStay.Areas.HotelManager.Controllers
{
    [Area("hotelManager")]
    [Authorize(Roles = "Hotel Manager")]
    public class AmentitiesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public AmentitiesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult AddAmentity(int id, IFormFile AmenityImage,string AmenityName)
        {
            var hotel = unitOfWork.hotelRepository.GetOne(filter: e => e.Id == id, includeprops: e => e.Include(e => e.HotelManager));
            if (hotel != null)
            {
                var NewAmentity = new Amentities();
                if (AmenityImage != null && AmenityImage.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(AmenityImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Amentities", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        AmenityImage.CopyTo(stream);
                    }
                    NewAmentity.Image = fileName;
                    NewAmentity.HotelId = id;
                    NewAmentity.Name = AmenityName; 
                    unitOfWork.AmentitiesRepository.Create(NewAmentity);
                    unitOfWork.Commit();
                    TempData["success"] = "Amentity added for hotel successfully";
                    return RedirectToAction("ShowAllHotels","ManageHotel",new { UserName = hotel.HotelManager.Name });
                }
            }
            TempData["success"] = "No Images added";
            return RedirectToAction("ShowAllHotels", "ManageHotel", new { UserName = hotel.HotelManager.Name });
        }
        public IActionResult Index(int id)
        {
            var hotel = unitOfWork.hotelRepository.GetOne(e=>e.Id==id,includeprops:e=>e.Include(e=>e.Amentities));
            return View(hotel);  
        }
        public IActionResult Delete(int id)
        {
            var hotelAmenity = unitOfWork.AmentitiesRepository.GetOne(e => e.Id == id);

            if (hotelAmenity == null)
            {
                return NotFound();
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Amentities", hotelAmenity.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            unitOfWork.AmentitiesRepository.Delete(hotelAmenity);
            unitOfWork.Commit();
            TempData["success"] = "Hotel Image Deleted Successfully";
            return RedirectToAction("Index", new { id = hotelAmenity.HotelId });
        }
        [HttpGet]
        public IActionResult Edit(int id) 
        {
          var Amenity = unitOfWork.AmentitiesRepository.GetOne(e => e.Id == id);
          return View(Amenity);
        }
        [HttpPost]
        public IActionResult Edit(Amentities amenity, IFormFile ImageFile)
        {
            var existingAmentity = unitOfWork.AmentitiesRepository.GetOne(e => e.Id == amenity.Id);
            if (existingAmentity == null)
            {
                return NotFound();
            }
            ModelState.Remove("ImageFile");
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Amentities", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        ImageFile.CopyTo(stream);
                    }
                    if (existingAmentity.Image != null)
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Amentities", existingAmentity.Image);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    existingAmentity.Image = fileName;
                }
                existingAmentity.Name = amenity.Name;
                existingAmentity.Description = amenity.Description;
                unitOfWork.AmentitiesRepository.Alter(existingAmentity);
                unitOfWork.Commit();
                TempData["success"] = "Amenity Edited Successfully";
                return RedirectToAction("index", new { id = existingAmentity.HotelId });
            }
            TempData["success"] = "No Amenity updates Added Successfully";
            return RedirectToAction("index", new { id = existingAmentity.HotelId });
        }
        }
}
