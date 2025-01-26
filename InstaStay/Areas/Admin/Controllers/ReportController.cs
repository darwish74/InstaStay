using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.IRepositories;
using Models.Models;

namespace InstaStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public ReportController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var reports=unitOfWork.ProblemReportRepository.Get().ToList();
            return View(reports);
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
