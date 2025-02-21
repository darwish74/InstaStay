using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InstaStay.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AiChatController : Controller
    {
        private readonly GeminiService _geminiService;

        public AiChatController(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAIResponse(string userMessage)
        {
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                return Json(new { response = "Please enter a message." });
            }

            var aiResponse = await _geminiService.GetAIResponseAsync(userMessage);
            return Json(new { response = aiResponse });
        }
    }
}
