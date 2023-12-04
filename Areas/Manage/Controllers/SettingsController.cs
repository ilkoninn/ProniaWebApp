using Microsoft.AspNetCore.Mvc;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult ShowInfo()
        {
            return View();
        }

        public IActionResult UpdateInfo()
        {
            return View();
        }

    }
}
