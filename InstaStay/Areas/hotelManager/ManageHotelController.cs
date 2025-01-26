using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
namespace InstaStay.Areas.hotelManager
{
    [Area("hotelManager")]
    [Authorize(Roles = "User")]
    public class ManageHotelController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ManageHotelController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }




        public IActionResult Index()
        {
            return View();
        }
    }
}
