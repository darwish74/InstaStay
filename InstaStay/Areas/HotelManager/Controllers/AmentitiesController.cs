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
    }
}
