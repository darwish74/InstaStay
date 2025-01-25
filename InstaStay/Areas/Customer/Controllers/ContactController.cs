using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using DataAccess;
using Models.IRepositories;
namespace InstaStay.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "User")]
    public class ContactController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ContactController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReportProblem(string problemDescription, string userId)
        {
            if (string.IsNullOrEmpty(problemDescription))
            {
                TempData["Error"] = "Please provide a description of the problem.";
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Invalid user ID.";
                return RedirectToAction("Index", "Home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Index", "Home");
            }

            var problemReport = new ProblemReport
            {
                UserName = user.UserName,
                UserEmail = user.Email,
                ProblemDescription = problemDescription,
                CreatedAt = DateTime.Now
            };

            try
            {
                _unitOfWork.ProblemReportRepository.Create(problemReport);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while submitting your problem report.";
                // Log exception here
                return RedirectToAction("Index", "Home");
            }

            TempData["Success"] = "Your problem report has been submitted successfully.";
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> RequestConversion(string managerReason)
        {
            if (string.IsNullOrEmpty(managerReason))
            {
                TempData["Error"] = "Please provide a reason for requesting a manager account.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Your account conversion request has been submitted successfully.";
            return RedirectToAction("Index");
        }
    }
}