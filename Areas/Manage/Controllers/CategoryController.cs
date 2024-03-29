﻿

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
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Table()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.categories = await _db.Categories.ToListAsync();
            return View(adminVM);
        }


        // <--- Create Section --->
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
                UpdatedDate = DateTime.Now,
            };

            _db.Categories.Add(newCategory);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int Id)
        {
            Category oldCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == Id);
            if (oldCategory == null) return RedirectToAction("NotFound", "AdminHome");

            CategoryVM categoryVM = new CategoryVM 
            {
                Name = oldCategory.Name,
            };

            return View(categoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryVM categoryVM)
        {

            var existsName = await _db.Categories.Where(cat => cat.Name == categoryVM.Name && cat.Id != categoryVM.Id).FirstOrDefaultAsync() != null;


            if (existsName)
            {
                ModelState.AddModelError("Name", "There is a same name category in Table!");
                return View(categoryVM);
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            Category oldCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == categoryVM.Id);
            if (oldCategory == null) return RedirectToAction("NotFound", "AdminHome"); 
            
            oldCategory.Name = categoryVM.Name;
            oldCategory.UpdatedDate = DateTime.Now;
            oldCategory.CreatedDate = oldCategory.CreatedDate;

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            Category oldCategory = await _db.Categories.FirstOrDefaultAsync(x => x.Id == Id);
            if (oldCategory == null) return RedirectToAction("NotFound", "AdminHome");

            _db.Categories.Remove(oldCategory);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
    }
}
