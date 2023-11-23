

namespace ProniaWebApp.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM vm = new HomeVM(); 
            vm.sliders = _context.Sliders.ToList();
            return View(vm);
        }
    }
}
