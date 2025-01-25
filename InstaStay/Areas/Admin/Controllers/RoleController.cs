using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels;

namespace InstaStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
     
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Role()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                TempData["Error"] = "User or role is not valid.";
                return RedirectToAction("Index");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Index");
            }
            var currentRoles = await userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await userManager.RemoveFromRolesAsync(user, currentRoles);
            }
            var result = await userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                TempData["Success"] = "Role assigned successfully.";
            }
            else
            {
                TempData["Error"] = "Error occurred while assigning role.";
            }
            return RedirectToAction("Index","User");
        }

        [HttpPost]
        public async Task<IActionResult> Role(RoleVM role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole IdentityRole = new();
                IdentityRole.Name = role.RoleName;
                var result = await roleManager.CreateAsync(IdentityRole);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Saved Successfully";
                    return View();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(role);
        }
    }
}

