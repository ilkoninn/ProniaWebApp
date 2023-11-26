
using ProniaWebApp.Areas.Manage.ViewModels;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AdminHomeController : Controller
    {
        AppDbContext _db;
        public AdminHomeController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
