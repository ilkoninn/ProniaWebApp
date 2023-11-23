using Microsoft.AspNetCore.Mvc;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TableController : Controller
    {
        AppDbContext _db;
        public TableController(AppDbContext appDb)
        {
            _db = appDb;
        }
        public async Task<IActionResult> CategoryTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.categories = await _db.Categories.ToListAsync();
            return View(adminVM);
        }
        public async Task<IActionResult> TagTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.tags = await _db.Tags.ToListAsync();
            
            return View(adminVM);
        }
        public async Task<IActionResult> ProductTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.products = await _db.Products
                .Include(x => x.Category)
                .ToListAsync();
            
            return View(adminVM);
        }
        public async Task<IActionResult> BlogTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.blogs = await _db.Blogs
                .Include(x => x.Category)
                .ToListAsync();

            return View(adminVM);
        }
        public async Task<IActionResult> SliderTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.slider = await _db.Sliders.ToListAsync();

            return View(adminVM);
        }
        public async Task<IActionResult> BlogImagesTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.blogImages = await _db.BlogsImages
                .Include(x => x.Blog)
                .ToListAsync();

            return View(adminVM);
        }
        public async Task<IActionResult> ProductImagesTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.productImages = await _db.ProductImages
                .Include(x => x.Product)
                .ToListAsync();

            return View(adminVM);
        }
    }
}
