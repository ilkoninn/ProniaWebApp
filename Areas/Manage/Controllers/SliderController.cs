
using Azure;
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext appDb, IWebHostEnvironment env)
        {
            _db = appDb;
            _env = env;
        }

        // <--- Table Section --->
        public async Task<IActionResult> Table()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.sliders = await _db.Sliders.ToListAsync();

            return View(adminVM);
        }


        // <--- Create Section --->
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Slider Slider)
        {
            var existsImgFile = Slider.ImageFile == null;
            var existsTitle = await _db.Sliders.FirstOrDefaultAsync(x => x.Title == Slider.Title) != null;
            var existsSubTitle = await _db.Sliders.FirstOrDefaultAsync(x => x.SubTitle == Slider.SubTitle) != null;
            var existsDescription = await _db.Sliders.FirstOrDefaultAsync(x => x.Description == Slider.Description) != null;

            if (existsImgFile)
            {
                ModelState.AddModelError("ImageFile", "Image should be uploaded!");
            }
            if (existsTitle)
            {
                ModelState.AddModelError("Title", "There is a same title slider in Table!");
            }
            if (existsSubTitle)
            {
                ModelState.AddModelError("SubTitle", "There is a same sub title slider in Table!");
            }
            if (existsDescription)
            {
                ModelState.AddModelError("Description", "There is a same description slider in Table!");
            }

            if(ModelState.ErrorCount > 0)
            {
                return View(Slider);
            }



            if (!Slider.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "You can upload only images");
                return View(Slider);
            }
            if (Slider.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                return View(Slider);
            }

            Slider.ImgUrl = Slider.ImageFile.Upload(_env.WebRootPath, @"\Upload\SliderImages\");

            if (!ModelState.IsValid)
            {
                return View(Slider);
            }

            _db.Sliders.Add(Slider);
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

            Slider Slider = await _db.Sliders.FindAsync(Id);

            return View(Slider);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Slider newSlider)
        {
            Slider oldSlider = _db.Sliders.Find(newSlider.Id);

            var existsImgFile = newSlider.ImageFile == null;
            var existsTitle = await _db.Sliders.FirstOrDefaultAsync(x => x.Title == newSlider.Title) != null;
            var existsSubTitle = await _db.Sliders.FirstOrDefaultAsync(x => x.SubTitle == newSlider.SubTitle) != null;
            var existsDescription = await _db.Sliders.FirstOrDefaultAsync(x => x.Description == newSlider.Description) != null;

            if (existsImgFile)
            {
                ModelState.AddModelError("ImageFile", "Image should be uploaded!");
            }
            if (existsTitle)
            {
                ModelState.AddModelError("Title", "There is a same title slider in Table!");
            }
            if (existsSubTitle)
            {
                ModelState.AddModelError("SubTitle", "There is a same sub title slider in Table!");
            }
            if (existsDescription)
            {
                ModelState.AddModelError("Description", "There is a same description slider in Table!");
            }

            if (ModelState.ErrorCount > 0)
            {
                return View(newSlider);
            }

            if (!newSlider.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "You can upload only images");
                return View();
            }
            if (newSlider.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                return View();
            }

            FileManager.Delete(oldSlider.ImgUrl, _env.WebRootPath, @"\Upload\SliderImages\");
            newSlider.ImgUrl = newSlider.ImageFile.Upload(_env.WebRootPath, @"\Upload\SliderImages\");

            if (!ModelState.IsValid)
            {
                return View();
            }

            oldSlider.Title = newSlider.Title;
            oldSlider.SubTitle = newSlider.SubTitle;
            oldSlider.Description = newSlider.Description;
            oldSlider.ImgUrl = newSlider.ImgUrl;

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        public async Task<IActionResult> Delete(int Id)
        {
            Slider Slider = await _db.Sliders.FindAsync(Id);
            _db.Sliders.Remove(Slider);
            await _db.SaveChangesAsync();
            FileManager.Delete(Slider.ImgUrl, _env.WebRootPath, @"\Upload\SliderImages\");

            return RedirectToAction("Table");
        }
    }
}
