using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public SettingsController(AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _env = webHostEnvironment;
        }

        public async Task<IActionResult> ShowInfo()
        {
            var settings = await _db.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);

            if(settings != null)
            {
                return View(settings);
            }
            else
            {
                return RedirectToAction("UpdateInfo");
            }

        }

        public async Task<IActionResult> UpdateInfo(SettingsVM settings)
        {
            foreach (var item in await _db.Settings.ToDictionaryAsync(s => s.Key, s => s.Value))
            {
                
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("ShowInfo");
        }

    }
}
