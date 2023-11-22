using Microsoft.AspNetCore.Mvc;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CRUDCategoryController : Controller
    {
        AppDbContext _db;
        public CRUDCategoryController(AppDbContext appDb)
        {
            _db = appDb;
        }
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("CategoryTable", "Table");
        }
        public IActionResult UpdateCategory(int Id)
        {
            Category category = _db.Categories.Find(Id);
            return View(category);
        }
        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            Category oldCategory = _db.Categories.Find(category.Id);
            oldCategory.Name = category.Name;
            _db.SaveChanges();
            return RedirectToAction("CategoryTable", "Table");
        }
        public IActionResult DeleteCategory(int Id)
        {
            Category category = _db.Categories.Find(Id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
            return RedirectToAction("CategoryTable", "Table");
        }
    }
}
