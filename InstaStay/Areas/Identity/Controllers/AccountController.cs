using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Models.Models;
using Models.ViewModels;
using Mono.TextTemplating;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace InstaStay.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager; 
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View();
        }

        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Register));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Register));
            }

            // Sign in the user with this external login provider if the user already has a login
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            if (signInResult.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(LoginWith2fa), new { ReturnUrl = returnUrl });
            }

            if (signInResult.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }

            // If the user does not have an account, then create one and sign in
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                var user = new ApplicationUser { UserName = email, Email = email };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
            }

            ViewData["ErrorMessage"] = "Error signing in with external provider.";
            return RedirectToAction(nameof(Register));
        }



        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUserVM userVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser User = new()
                {
                    UserName = userVM.UserName,
                    Email = userVM.Email,
                    FirstName = userVM.FirstName,
                    LastName = userVM.LastName,
                    City = userVM.City,
                    State = userVM.State,
                    Country = userVM.Country,
                    PostalCode = userVM.PostalCode,
                };
                var result = await _userManager.CreateAsync(User, userVM.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(User, "Admin");

                    await _signInManager.SignInAsync(User, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(userVM);
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            ViewBag.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM LoginVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(LoginVM.Name);
                if (user != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(user, LoginVM.Password);
                    if (found)
                    {
                        await _signInManager.SignInAsync(user, LoginVM.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "UserName Or Password wrong");
            }
            return View();
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(new ApplicationUserVM()
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                State = user.State,
                Country = user.Country,
                PostalCode = user.PostalCode,
            });
        }
        [HttpPost]
        public async Task<IActionResult> Profile(ApplicationUserVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.City = model.City;
            user.State = model.State;
            user.Country = model.Country;
            user.PostalCode = model.PostalCode;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim("UserName", user.UserName));
                await _signInManager.RefreshSignInAsync(user);
                TempData["success"] = "Profile updated successfully.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

    }
}