// Updated UserController.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.ViewModels;
using X.PagedList;
using X.PagedList.Extensions;

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

        public IActionResult Index(string? Account, int pageNumber = 1, int pageSize = 10)
        {
            var usersQuery = userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(Account))
            {
                usersQuery = usersQuery.Where(u => u.Email.Contains(Account) || u.UserName.Contains(Account));
            }
            int totalUsers = usersQuery.Count();
            var users = usersQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var viewModel = new UserPaginationViewModel
            {
                Users = users,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalUsers = totalUsers
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> SearchForUser(string account)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", new { Account = account });

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> BlockUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await userManager.SetLockoutEnabledAsync(user, true);
                var result = await userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(100));
                if (result.Succeeded)
                {
                    user.ISBlocked = true;
                    await userManager.UpdateAsync(user);
                    TempData["success"] = "The user has been blocked successfully.";
                }
                else
                {
                    TempData["error"] = "The user could not be blocked.";
                }
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["success"] = "The user has been deleted successfully.";
                }
                else
                {
                    TempData["error"] = "The user could not be deleted.";
                }
            }
            else
            {
                TempData["error"] = "User not found.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnBlockUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await userManager.SetLockoutEndDateAsync(user, null);
                await userManager.SetLockoutEnabledAsync(user, false);
                if (result.Succeeded)
                {
                    user.ISBlocked = false;
                    await userManager.UpdateAsync(user);
                    TempData["success"] = "The user has been unblocked successfully.";
                }
                else
                {
                    TempData["error"] = "The user could not be unblocked.";
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
