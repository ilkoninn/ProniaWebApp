
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
            if (!ModelState.IsValid)
            {
                return View();
            }
            _db.Tags.Add(Tag);
            _db.SaveChanges();

            return RedirectToAction("TagTable", "Table");
        }
        public IActionResult UpdateTag(int Id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Tag Tag = _db.Tags.Find(Id);

            return View(Tag);
        }
        [HttpPost]
        public IActionResult UpdateTag(Tag newTag)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Tag oldTag = _db.Tags.Find(newTag.Id);
            oldTag.Name = newTag.Name;
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
