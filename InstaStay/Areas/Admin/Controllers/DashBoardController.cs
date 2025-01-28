using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;
using Models.ViewModels;
namespace InstaStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashBoardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork unitOfWork;

        public DashBoardController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            this.unitOfWork = unitOfWork;
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
        [HttpPost]  
        public IActionResult Accept(int id)
        {
            var request = unitOfWork.NewHotelRequestsRepository.GetOne(e => e.Id == id);
            var hotelManager = unitOfWork.HotelManagerRepository.GetOne(r => r.Name == request.HotelManager);
            
            
            var hotel = new Hotel
            {
                Name = request.Name,
                Address = request.Address,
                Description = request.Description,
                CoverImage = request.CoverImage,
                Stars = request.stars,
                HotelManager= hotelManager
            };
            unitOfWork.hotelRepository.Create(hotel);
            unitOfWork.Commit();
            unitOfWork.NewHotelRequestsRepository.Delete(request);
            unitOfWork.Commit();
            TempData["success"] = "New Hotel Added Successfully";
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var request = unitOfWork.NewHotelRequestsRepository.GetOne(e => e.Id == id);
            unitOfWork.NewHotelRequestsRepository.Delete(request);
            unitOfWork.Commit();
            TempData["success"] = "Request Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
