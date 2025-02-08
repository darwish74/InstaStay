using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.IRepositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InstaStay.Areas.Manager.Controllers
{
    [Area("HotelManager")]
    public class CouponController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CouponController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var coupons = unitOfWork.CouponRepository.Get().ToList();
            return View(coupons);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                coupon.IsActive = true;
                unitOfWork.CouponRepository.Create(coupon);
                unitOfWork.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }
        //public IActionResult Edit(int id)
        //{
        //    var coupon = unitOfWork.CouponRepository.GetOne(c => c.Id == id);
        //    if (coupon == null) return NotFound();
        //    return View(coupon);
        //}
        [HttpPost]
        public async Task<IActionResult> Edit(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.CouponRepository.Alter(coupon);
                unitOfWork.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }
        //public IActionResult Delete(int id)
        //{
        //    var coupon = unitOfWork.CouponRepository.GetOne(c => c.Id == id);
        //    if (coupon == null) return NotFound();
        //    return View(coupon);
        //}
        //[HttpPost, ActionName("Delete")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var coupon = unitOfWork.CouponRepository.GetOne(c => c.Id == id);
        //    if (coupon != null)
        //    {
        //        unitOfWork.CouponRepository.Delete(coupon);
        //        unitOfWork.Commit();
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
        //public IActionResult ToggleStatus(int id)
        //{
        //    var coupon = unitOfWork.CouponRepository.GetOne(c => c.Id == id);
        //    if (coupon == null) return NotFound();

        //    coupon.IsActive = !coupon.IsActive;
        //    unitOfWork.CouponRepository.Update(coupon);
        //    unitOfWork.Commit();

        //    return RedirectToAction(nameof(Index));
        //}
    }
}
