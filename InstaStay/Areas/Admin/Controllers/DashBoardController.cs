using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaStay.Areas.Admin.Controllers
{
    [Area("Admin")]
  //  [Authorize(Roles = "User")]
    public class DashBoardController : Controller
    {   
        public IActionResult Index()
        {
            return View();
        }
    }
}
