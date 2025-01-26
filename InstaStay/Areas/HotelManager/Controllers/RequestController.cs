using Microsoft.AspNetCore.Mvc;

namespace InstaStay.Areas.hotelManager.Controllers
{
    public class RequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
