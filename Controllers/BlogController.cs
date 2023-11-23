using Microsoft.AspNetCore.Mvc;

namespace ProniaWebApp.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult BlogList()
        {
            return View();
        }
    }
}
