


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
        [Authorize(Roles = "Admin, Moderator")]
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
