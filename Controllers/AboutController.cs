using Microsoft.AspNetCore.Mvc;

namespace ProniaWebApp.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
