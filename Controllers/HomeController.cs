

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
            HomeVM homeVM = new HomeVM();
            homeVM.sliders = _context.Sliders.ToList();
            homeVM.products = _context.Products
                .Include(x => x.ProductImage)
                .ToList();
            homeVM.blogs = _context.Blogs
                .Include(x => x.BlogImage)
                .ToList();

            return View(homeVM);
        }
    }
}
