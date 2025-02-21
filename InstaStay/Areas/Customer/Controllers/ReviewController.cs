using DataAccess;
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
        private void LogActivity(string userName, string description)
        {
            var activity = new ActivityLog
            {
                UserName = userName,
                Description = description,
                Date = DateTime.Now
            };

            unitOfWork.ActivityLogRepository.Create(activity);
            unitOfWork.Commit();
        }
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

            LogActivity(user.UserName, $"Added a review for hotel {model.HotelId} with a rating of {model.Rate}.");

            TempData["success"] = "Review Added Successfully";
            return RedirectToAction("HotelDetails", "BrowseHotel", new { hotelId = model.HotelId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var review = unitOfWork.reviewRepository.GetOne(r => r.Id == reviewId && r.UserId == user.Id);
            if (review == null)
            {
                return NotFound();
            }

            unitOfWork.reviewRepository.Delete(review);
            unitOfWork.Commit();
            LogActivity(user.UserName, $"Deleted a review (ID: {reviewId}) for hotel {review.HotelId}.");
            TempData["success"] = "Review Deleted Successfully";
            return RedirectToAction("HotelDetails", "BrowseHotel", new { hotelId = review.HotelId });
        }
    }
}
