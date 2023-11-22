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
        public IActionResult CategoryTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.categories = _db.Categories.ToList();
            return View(adminVM);
        }
        public IActionResult TagTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.tags = _db.Tags.ToList();
            
            return View(adminVM);
        }
        public IActionResult ProductTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.products = _db.Products
                .Include(x => x.Category)
                .ToList();
            
            return View(adminVM);
        }
        public IActionResult BlogTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.blogs = _db.Blogs
                .Include(x => x.Category)
                .ToList();
            return View(adminVM);
        }
        public IActionResult SliderTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.slider = _db.Sliders.ToList();
            return View(adminVM);
        }
        public IActionResult BlogImagesTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.blogImages = _db.BlogsImages
                .Include(x => x.Blog)
                .ToList();
            return View(adminVM);
        }
        public IActionResult ProductImagesTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.productImages = _db.ProductImages
                .Include(x => x.Product)
                .ToList();
            return View(adminVM);
        }
    }
}
