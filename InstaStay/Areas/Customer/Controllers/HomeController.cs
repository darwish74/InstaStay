using Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Models.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InstaStay.Areas.Customer.Controllers
{    
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
            var hotels = unitOfWork.hotelRepository.Get(includeprops: e => e.Include(e => e.HotelManager).Include(e => e.Rooms).Include(e=>e.Amentities));
            ViewBag.HotelAddresses = unitOfWork.hotelRepository.Get()
               .Select(h => new SelectListItem { Value = h.Address, Text = h.Address })
               .Distinct()
               .ToList();
            ViewBag.Amenities = unitOfWork.AmentitiesRepository.Get().ToList();
            return View(hotels);
        }
        [HttpGet]
        public IActionResult Search(string Address, List<int> Amenities, double? MinPrice, double? MaxPrice)
        {
            var hotels = unitOfWork.hotelRepository.Get(includeprops: e => e.Include(h => h.Amentities).Include(e => e.Rooms));

            if (!string.IsNullOrEmpty(Address))
            {
                hotels = hotels.Where(h => h.Address == Address);
            }

            if (Amenities != null && Amenities.Any())
            {
                hotels = hotels.Where(h => Amenities.All(a => h.Amentities.Any(ha => ha.Id == a)));
            }

            if (MinPrice.HasValue)
            {
                hotels = hotels.Where(h => h.Rooms.Any(e => e.PricePerNight >= MinPrice));
            }

            if (MaxPrice.HasValue)
            {
                hotels = hotels.Where(h => h.Rooms.Any(e => e.PricePerNight <= MaxPrice));
            }
            ViewBag.HotelAddresses = unitOfWork.hotelRepository.Get()
                .Select(h => new SelectListItem { Value = h.Address, Text = h.Address })
                .Distinct()
                .ToList();
            ViewBag.Amenities = unitOfWork.AmentitiesRepository.Get()
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                .ToList();
            return View("Index", hotels.ToList());
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
