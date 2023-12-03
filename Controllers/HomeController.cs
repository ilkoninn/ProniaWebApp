

namespace ProniaWebApp.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new HomeVM(); 
            vm.sliders = await _context.Sliders
                .ToListAsync();
            vm.products = await _context.Products
                .Include(x => x.ProductImage)
                .ToListAsync();
            vm.blogs = await _context.Blogs
                .Include(x => x.BlogImage)
                .ToListAsync();
            
            return View(vm);
        }
    }
}
