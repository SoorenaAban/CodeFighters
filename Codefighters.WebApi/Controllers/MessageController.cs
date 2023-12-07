using Microsoft.AspNetCore.Mvc;

namespace CodeFighters.WebApi.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
