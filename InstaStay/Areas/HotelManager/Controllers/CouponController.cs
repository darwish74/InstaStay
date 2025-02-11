using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.IRepositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace InstaStay.Areas.Manager.Controllers
{
    [Area("HotelManager")]
    [Authorize(Roles = "Hotel Manager")]
    public class CouponController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public CouponController(IUnitOfWork unitOfWork ,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var coupons = unitOfWork.CouponRepository.GetCouponsByManager(user.Id);
            return View(coupons);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Coupon coupon)
        {
            var user = await userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                coupon.IsActive = true;
                coupon.HotelManagerId = user.Id; 
                unitOfWork.CouponRepository.Create(coupon);
                unitOfWork.Commit();
                TempData["success"] = "Coupon Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var coupon = unitOfWork.CouponRepository.GetOne(c => c.CouponId == id && c.HotelManagerId == user.Id);
            if (coupon == null) return NotFound();

            return View(coupon);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                coupon.HotelManagerId = user.Id;
                unitOfWork.CouponRepository.Alter(coupon);
                unitOfWork.Commit();
                TempData["success"] = "Coupon Edited Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var coupon = unitOfWork.CouponRepository.GetOne(c => c.CouponId == id && c.HotelManagerId == user.Id);
            if (coupon == null) return NotFound();

            return View(coupon);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int CouponId)
        {
            var user = await userManager.GetUserAsync(User);
            var coupon = unitOfWork.CouponRepository.GetOne(c => c.CouponId == CouponId && c.HotelManagerId == user.Id);

            if (coupon != null)
            {
                unitOfWork.CouponRepository.Delete(coupon);
                unitOfWork.Commit();
                TempData["success"] = "Coupon Deleted successfully";
            }
            TempData["success"] = "Coupon Deleted successfully";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ToggleStatus(int id)
        {
            var coupon = unitOfWork.CouponRepository.GetOne(c => c.CouponId == id);
            if (coupon == null) return NotFound();

            coupon.IsActive = !coupon.IsActive;
            unitOfWork.CouponRepository.Alter(coupon);
            unitOfWork.Commit();

            return RedirectToAction(nameof(Index));
        }
    }
}
