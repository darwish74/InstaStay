using Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Models.IRepositories;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Index()
        {
            var hotels=unitOfWork.hotelRepository.Get(includeprops:e=>e.Include(e=>e.HotelManager));
            return View(hotels);
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
