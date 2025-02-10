using InstaStay.HubChat;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.IRepositories;
using Models.Models;
using Newtonsoft.Json;
using Utilities.Utility;
using Microsoft.AspNetCore.Authorization;


namespace InstaStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ReportController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<ChatHub> hubContext;
        private readonly ILogger<ProblemReport> logger;

        public ReportController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
            IHubContext<ChatHub> hubContext, ILogger<ProblemReport> logger)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.hubContext = hubContext;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            var reports=unitOfWork.ProblemReportRepository.Get().OrderByDescending(a => a.Id);
            return View(reports);
        }

        public IActionResult Details(int reqId)
        {
            var req = unitOfWork.ProblemReportRepository.GetOne( e => e.Id == reqId);
            if (req == null)
            {
                return RedirectToAction("NotFound", "Home", new { area = "Customer" });
            }
            
            
            return View(req);
        }
        [HttpPost]
        public async Task<IActionResult> Respond(Message message)
        {
            message.MessageDateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                unitOfWork.MessageRepository.Create(message);
                await unitOfWork.CommitAsync();

                // Notify clients
                var messageInfo = JsonConvert.SerializeObject(new
                {
                    message.Id,
                    message.Title,
                    message.Description,
                    message.MessageDateTime,
                    message.UserId
                });
                var userMessageCount = unitOfWork.MessageRepository.Get(m => m.UserId == message.UserId
                 );

                await hubContext.Clients.Group("Customers").SendAsync("CustomerNotification", messageInfo, userMessageCount, message.UserId);

                Log(nameof(Respond), nameof(message) + " " + $"{message.Title}");
                TempData["success"] = "Response sent successfully.";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Failed to send response. Please check your input.";
            return RedirectToAction("Details", new { reqId = message.UserId });
        }

        public async Task<IActionResult> Delete(int reqId)
        {
            var toDelete = unitOfWork.ProblemReportRepository.GetOne( e => e.Id == reqId);
            if (toDelete != null)
            {
                unitOfWork.ProblemReportRepository.DeleteWithImage(toDelete, "ContactUsImages", toDelete.UserImgRequest);
                unitOfWork.Commit();
                Log(nameof(Delete), "Request" + " " + $"{toDelete.ProblemDescription}");
                TempData["success"] = "Message deleted successfully.";

            }
            return RedirectToAction("Index");
        }




        public async void Log(string action, string entity)
        {
            LoggerHelper.LogAdminAction(logger, User.Identity.Name, action, entity);

        }

        public IActionResult Requests()
        {
            var Requests = unitOfWork.HotelManagerRequestsRepository.Get().ToList();
            return View(Requests);
        }
        public async Task<IActionResult> AcceptRequest(string userEmail, int id)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            var currentRoles = await userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await userManager.RemoveFromRolesAsync(user, currentRoles);
            }
            var result = await userManager.AddToRoleAsync(user, "Hotel Manager");
            var HotelManager=new HotelManagers() {
            Name=user.UserName
            };
            unitOfWork.HotelManagerRepository.Create(HotelManager);
            unitOfWork.Commit();
            var request = unitOfWork.HotelManagerRequestsRepository.GetOne(r => r.Id == id);
            unitOfWork.HotelManagerRequestsRepository.Delete(request);
            unitOfWork.Commit();
            TempData["Success"] = "Request accepted, role assigned, and request deleted successfully.";
            return RedirectToAction("Requests");
        }
        public IActionResult DeleteRequest(int id)
        {
            var request = unitOfWork.HotelManagerRequestsRepository.GetOne(r => r.Id == id);
            if (request != null)
            {
                unitOfWork.HotelManagerRequestsRepository.Delete(request);
                unitOfWork.Commit();
                TempData["Success"] = "Request deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Request not found.";
            }

            return RedirectToAction("Requests");
        }
        public IActionResult HotelRequestes()
        {
            var NewHotelReport=unitOfWork.NewHotelRequestsRepository.Get().ToList();
            
            return View(NewHotelReport);
        }

    }
}
