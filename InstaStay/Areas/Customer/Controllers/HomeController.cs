using Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Models.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InstaStay.Areas.Customer.Controllers
{     //includeprops: e => e.Include(e => e.category)
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var hotels = unitOfWork.hotelRepository.Get(includeprops: e => e.Include(e => e.HotelManager));
            ViewBag.HotelAddresses = hotels.Select(h => new SelectListItem
            {
                Value = h.Address,
                Text = h.Address
            }).Distinct().ToList();

            return View(hotels);
        }

        [HttpGet]
        public IActionResult Search(string Address)
        {
            var hotels = unitOfWork.hotelRepository.Get(filter: e => e.Address == Address).ToList();

            ViewBag.HotelAddresses = unitOfWork.hotelRepository.Get()
                .Select(h => new SelectListItem { Value = h.Address, Text = h.Address })
                .Distinct()
                .ToList();

            return View("Index", hotels);
        }


















        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
