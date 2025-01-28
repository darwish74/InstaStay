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
            var Hotels = unitOfWork.hotelRepository.Get(includeprops:e=>e.Include(e=>e.Rooms).Include(e=>e.HotelManager));
            return View(Hotels);
        }
    }
}
