
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
        public async Task<IActionResult> Create(TagVM tagVM)
        {
            if (await _db.Tags.FirstOrDefaultAsync(x => x.Name == tagVM.Name) != null)
            {
                ModelState.AddModelError("Name", "There is a same name Tag in Table!");
                return View(tagVM);
            }

            if (!ModelState.IsValid)
            {
                return View(tagVM);
            }

            Tag newTag = new Tag();
            newTag.Name = tagVM.Name;

            _db.Tags.Add(newTag);
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

            Tag oldTag = await _db.Tags.FindAsync(Id);
            TagVM tagVM = new TagVM
            {
                Name = oldTag.Name
            };

            return View(tagVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TagVM tagVM)
        {
            if (await _db.Tags.FirstOrDefaultAsync(x => x.Name == tagVM.Name) != null)
            {
                ModelState.AddModelError("Name", "There is a same name Tag in Table!");
                return View(tagVM);
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            Tag oldTag = await _db.Tags.FindAsync(tagVM.Id);
            oldTag.Name = tagVM.Name;
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        public async Task<IActionResult> Delete(int Id)
        {
            Tag tag = await _db.Tags.FindAsync(Id);
            _db.Tags.Remove(tag);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
    }
}
