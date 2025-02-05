using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;
using Models.ViewModels;

namespace InstaStay.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public ReviewController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewVM model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("HotelDetails", "BrowseHotel", new { id = model.HotelId });
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["success"] = "You Can't Add Review Please First Login or Register";
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            else
            {
                var review = new Review
                {
                    UserId = user.Id,
                    User = user,
                    HotelId = model.HotelId,
                    Rate = model.Rate,
                    ReviewTitle = model.ReviewTitle,
                    ReviewText = model.ReviewText,
                    ReviewDate = DateTime.Now
                };
                unitOfWork.reviewRepository.Create(review);
                unitOfWork.Commit();
                TempData["success"] = "Review Added Successfully";
                return RedirectToAction("HotelDetails", "BrowseHotel", new { hotelId = model.HotelId });
            }
        }
    }
}
