using NuGet.Protocol;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _db;

        public SettingsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> ShowInfo()
        {
            if(await _db.Settings.ToListAsync() != null) 
            {
                return View();
            }
            else
            {
                return RedirectToAction("UpdateInfo", "Settings");
            }
        }

        public async Task<IActionResult> UpdateInfo(Dictionary<string, string> settings, IFormFile logo)
        {
            foreach (var item in settings)
            {
                var setting = await _db.Settings.FirstOrDefaultAsync(s => s.Key == item.Key);
                setting.Value = setting.Value;
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
