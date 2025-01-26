using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.IRepositories;
using Models.Models;
namespace InstaStay.Areas.hotelManager.Controllers
{
    [Area("hotelManager")]
    [Authorize(Roles = "User")]
    public class ManageHotelController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork unitOfWork;

        public ManageHotelController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpGet]
        public IActionResult NewHotelRequest()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewHotelRequest(NewHotelRequests request)
        {

            if(ModelState.IsValid)
            {

            }
            return View();
        }
    }
}
