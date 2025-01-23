using Microsoft.AspNetCore.Mvc;
namespace InstaStay.Areas.hotelManager
{
    public class ManageHotelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
