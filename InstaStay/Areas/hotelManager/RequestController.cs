using Microsoft.AspNetCore.Mvc;

namespace InstaStay.Areas.hotelManager
{
    public class RequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
