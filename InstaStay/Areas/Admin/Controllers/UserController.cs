using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

namespace InstaStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index(string? Account)
        {
            if (Account == null)
            {
                return View(userManager.Users.ToList());
            }

            var Users = userManager.Users.Where(e =>
                 e.Email.Contains(Account) || e.UserName.Contains(Account)).ToList();
            if (Users.Any())
            {

                return View(Users);
            }

            return View();
        }


        public async Task<IActionResult> BlockUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await userManager.SetLockoutEnabledAsync(user, true);
                var result = await userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(100));
                //await userManager.SetLockoutEnabledAsync(user, false);
                if (result.Succeeded)
                {
                    user.ISBlocked = true;
                    await userManager.UpdateAsync(user);
                    TempData["success"] = "The user has been blocked succefully";
                    var allUser = userManager.Users.ToList();
                    return View("Index", allUser);
                }
                TempData["error"] = "The user has not been blocked";
                var allUser2 = userManager.Users.ToList();
                return View("Index", allUser2);
            }
            return RedirectToAction("NotFoundSearh");
        }

        public async Task<IActionResult> unBlockUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //await userManager.SetLockoutEnabledAsync(user, true);
                var result = await userManager.SetLockoutEndDateAsync(user, null);
                await userManager.SetLockoutEnabledAsync(user, false);

                if (result.Succeeded)
                {
                    user.ISBlocked = false;
                    await userManager.UpdateAsync(user);
                    TempData["success"] = "The user is not blocked from thd site";
                    var allUser = userManager.Users.ToList();
                    return View("Index", allUser);
                }
                TempData["error"] = "something  has happened error";

                var allUser2 = userManager.Users.ToList();
                return View("Index", allUser2);
            }
            return RedirectToAction("index", "home");
        }

    }
}
