
using ProniaWebApp.Areas.Manage.ViewModels;
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        public CategoryController(AppDbContext appDb)
        {
            _db = appDb;
        }

        // <--- Table Section --->
        public async Task<IActionResult> Table()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.categories = await _db.Categories.ToListAsync();
            return View(adminVM);
        }


        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryVM categoryVM)
        {
            if (await _db.Categories.FirstOrDefaultAsync(x => x.Name == categoryVM.Name) != null)
            {
                ModelState.AddModelError("Name", "There is a same name category in Table!");
                return View(categoryVM);
            }
            
            if (!ModelState.IsValid)
            {
                return View(categoryVM);
            }

            Category newCategory = new Category 
            {
                Name = categoryVM.Name,
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
            };

            _db.Categories.Add(newCategory);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            Category oldCategory = await _db.Categories.FindAsync(Id);
            CategoryVM categoryVM = new CategoryVM 
            {
                Name = oldCategory.Name,
            };

            return View(categoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryVM categoryVM)
        {
            if (await _db.Categories.FirstOrDefaultAsync(x => x.Name == categoryVM.Name) != null)
            {
                ModelState.AddModelError("Name", "There is a same name category in Table!");
                return View(categoryVM);
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            Category oldCategory = await _db.Categories.FindAsync(categoryVM.Id);
            oldCategory.Name = categoryVM.Name;
            oldCategory.LastUpdatedDate = DateTime.Now;
            oldCategory.CreatedDate = oldCategory.CreatedDate;

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        public async Task<IActionResult> Delete(int Id)
        {
            Category Category = await _db.Categories.FindAsync(Id);
            _db.Categories.Remove(Category);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
    }
}
