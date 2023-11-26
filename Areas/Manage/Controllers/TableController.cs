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
       
        public async Task<IActionResult> BlogTable()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.blogs = await _db.Blogs
                .Include(x => x.Category)
                .ToListAsync();

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
        
    }
}
