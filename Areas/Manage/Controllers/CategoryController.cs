
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
        public async Task<IActionResult> Create(Category Category)
        {
            if(await _db.Categories.FirstOrDefaultAsync(x => x.Name == Category.Name) != null)
            {
                ModelState.AddModelError("Name", "There is a same name category in Table!");
                return View(Category);
            }
            
            if (!ModelState.IsValid)
            {
                return View(Category);
            }

            _db.Categories.Add(Category);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Category category = await _db.Categories.FindAsync(Id);

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category newCategory)
        {
            if (await _db.Categories.FirstOrDefaultAsync(x => x.Name == newCategory.Name) != null)
            {
                ModelState.AddModelError("Name", "There is a same name category in Table!");
                return View(newCategory);
            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            Category oldCategory = await _db.Categories.FindAsync(newCategory.Id);
            oldCategory.Name = newCategory.Name;
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
