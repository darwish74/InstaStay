using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using DataAccess;
using Models.IRepositories;
using InstaStay.HubChat;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
namespace InstaStay.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ContactController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> hubContext;

        public ContactController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext)
        {
            this.userManager = userManager;
            _unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProblemReport problemReport, IFormFile ImgFile)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("NotFound", "Home", new { area = "Customer" });
            }
            problemReport.UserId = user.Id;
            problemReport.UserName = user.UserName;
            problemReport.UserEmail = user.Email;
            ModelState.Remove(nameof(ImgFile));
            if (ModelState.IsValid)
            {
                _unitOfWork.ProblemReportRepository.CreateWithImage(problemReport, ImgFile, "ContactImages", "UserImgRequest");
                var message = ConfirmationMessage(user.Id);
                _unitOfWork.MessageRepository.Create(message);
                _unitOfWork.Commit();

                // Create a detailed object for the notification
                var Chat = new
                {
                    problemReport.Id,
                    
                    problemReport.UserEmail,
                    RequestType = problemReport.Request.ToString(),
                    problemReport.ProblemDescription,
                    
                    problemReport.CreatedAt,
                    problemReport.PhoneNumber

                };
                // Convert the object to JSON string
                var notificationJson = JsonSerializer.Serialize(Chat);
                // Notify admin with detailed contact request info

                await hubContext.Clients.Group("Admins").SendAsync("AdminNotification", notificationJson);
                TempData["Success"] = "Your message has been submitted successfully.";
                return RedirectToAction("Index", "Home");
            }
            return View(model: problemReport);
        }


        private static Message ConfirmationMessage(string userId)
        {
            var message = new Message
            {
                UserId = userId,
                Title = "Contact Us",
                Description = "we will replay ASAP",
                MessageDateTime = DateTime.Now,
            };
            return message;
        }
        [HttpPost]
        public async Task<IActionResult> RequestConversion(string managerReason, string userId)
        {
            if (string.IsNullOrEmpty(managerReason))
            {
                TempData["Error"] = "Please provide a reason for requesting a manager account.";
                return RedirectToAction("Index");
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
            var ManagerReason = new HotelManagerRequests
            {
                UserName = user.UserName,
                Email = user.Email,
                RequestMesssage= managerReason,
                CreatedAt = DateTime.Now
            };
            try
            {
                _unitOfWork.HotelManagerRequestsRepository.Create(ManagerReason);
                _unitOfWork.Commit();   
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while submitting your problem report.";
                return RedirectToAction("Index", "Home");
            }
            TempData["Success"] = "Your account conversion request has been submitted successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}