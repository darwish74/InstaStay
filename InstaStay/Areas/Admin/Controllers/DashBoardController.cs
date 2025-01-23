using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Models.Models;
using Models.ViewModels;

namespace InstaStay.Areas.Admin.Controllers
{
    [Area("Admin")]
  //  [Authorize(Roles = "User")]
    public class DashBoardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DashBoardController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]               
        public IActionResult AddAdmin() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>AddAdmin(ApplicationUserVM userVM)
        {
                ApplicationUser User = new()
                {
                    UserName = userVM.UserName,
                    Email = userVM.Email,
                    FirstName = userVM.FirstName,
                    LastName = userVM.LastName,

                };
                var result = await _userManager.CreateAsync(User, userVM.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(User, "Admin");
                    TempData["success"] = "Admin Created Successfully";
                    return RedirectToAction("index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return View(userVM);  
        }
    }
}
