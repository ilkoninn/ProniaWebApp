
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TagController : Controller
    {
        private readonly AppDbContext _db;
        public TagController(AppDbContext appDb)
        {
            _db = appDb;
        }

        // <--- Table Section --->
        public async Task<IActionResult> Table()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.tags = await _db.Tags.ToListAsync();
            return View(adminVM);
        }


        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tag Tag)
        {
            if (await _db.Tags.FirstOrDefaultAsync(x => x.Name == Tag.Name) != null)
            {
                ModelState.AddModelError("Name", "There is a same name category in Table!");
                return View(Tag);
            }
            
            if (!ModelState.IsValid)
            {
                return View(Tag);
            }

            _db.Tags.Add(Tag);
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

            Tag Tag = await _db.Tags.FindAsync(Id);

            return View(Tag);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Tag newTag)
        {
            if (await _db.Tags.FirstOrDefaultAsync(x => x.Name == newTag.Name) != null)
            {
                ModelState.AddModelError("Name", "There is a same name category in Table!");
                return View(newTag);
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            Tag oldTag = await _db.Tags.FindAsync(newTag.Id);
            oldTag.Name = newTag.Name;
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        public async Task<IActionResult> Delete(int Id)
        {
            Tag Tag = await _db.Tags.FindAsync(Id);
            _db.Tags.Remove(Tag);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
    }
}
