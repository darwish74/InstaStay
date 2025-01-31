using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;
using System.Text.Unicode;

namespace InstaStay.Areas.HotelManager.Controllers
{
    [Area("hotelManager")]
    [Authorize(Roles = "Hotel Manager")]
    public class ImagesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ImagesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index(int id)
        {
            var hotel = unitOfWork.hotelRepository.GetOne(e=>e.Id==id,includeprops: e => e.Include(e => e.HotelImages));
            return View(hotel);
        }
        public IActionResult Delete(int id)
        {
            var hotelImage = unitOfWork.HotelImagesRepository.GetOne(e => e.Id == id);

            if (hotelImage == null)
            {
                return NotFound();
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/HIL", hotelImage.ImageURL);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            unitOfWork.HotelImagesRepository.Delete(hotelImage);
            unitOfWork.Commit();
            TempData["success"] = "Hotel Image Deleted Successfully";
            return RedirectToAction("Index", new {id=hotelImage.HotelId});   
        }
    }
}
