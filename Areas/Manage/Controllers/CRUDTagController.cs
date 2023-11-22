using Microsoft.AspNetCore.Mvc;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CRUDTagController : Controller
    {
        AppDbContext _db;
        public CRUDTagController(AppDbContext appDb)
        {
            _db = appDb;
        }
        public IActionResult CreateTag()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateTag(Tag Tag)
        {
            _db.Tags.Add(Tag);
            _db.SaveChanges();
            return RedirectToAction("TagTable", "Table");
        }
        public IActionResult UpdateTag(int Id)
        {
            Tag Tag = _db.Tags.Find(Id);
            return View(Tag);
        }
        [HttpPost]
        public IActionResult UpdateTag(Tag Tag)
        {
            Tag oldTag = _db.Tags.Find(Tag.Id);
            oldTag.Name = Tag.Name;
            _db.SaveChanges();
            return RedirectToAction("TagTable", "Table");
        }
        public IActionResult DeleteTag(int Id)
        {
            Tag Tag = _db.Tags.Find(Id);
            _db.Tags.Remove(Tag);
            _db.SaveChanges();
            return RedirectToAction("TagTable", "Table");
        }
    }
}
