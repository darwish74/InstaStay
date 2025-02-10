using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.IRepositories;
using Models.Models;

namespace InstaStay.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatController(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("NotFound", "Home", new { area = "Customer" });
            }
            var messages = unitOfWork.MessageRepository.Get( e => e.UserId == user.Id)
                .OrderByDescending(d => d.MessageDateTime);
            return View(messages);
        }
        public IActionResult Delete(int id)
        {
            var toDelete = unitOfWork.MessageRepository.GetOne( e => e.Id == id);
            if (toDelete != null)
            {
                unitOfWork.MessageRepository.Delete(toDelete);
                unitOfWork.Commit();
                TempData["success"] = "Message deleted successfully.";
            }
            return RedirectToAction("Index");
        }
    }
}
