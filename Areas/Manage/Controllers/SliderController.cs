
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
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SliderVM sliderVM)
        {
            var existsImgFile = sliderVM.ImageFile == null;
            var existsTitle = await _db.Sliders.FirstOrDefaultAsync(x => x.Title == sliderVM.Title) != null;
            var existsSubTitle = await _db.Sliders.FirstOrDefaultAsync(x => x.SubTitle == sliderVM.SubTitle) != null;
            var existsDescription = await _db.Sliders.FirstOrDefaultAsync(x => x.Description == sliderVM.Description) != null;

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
                return View(sliderVM);
            }



            if (!sliderVM.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "You can upload only images");
            }
            if (sliderVM.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
            }
            if (!ModelState.IsValid)
            {
                return View(sliderVM);
            }

            Slider newSlider = new Slider 
            {
                Title = sliderVM.Title,
                SubTitle = sliderVM.SubTitle,
                Description = sliderVM.Description,
                ImgUrl = sliderVM.ImageFile.Upload(_env.WebRootPath, @"\Upload\SliderImages\"),
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
            };

            _db.Sliders.Add(newSlider);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            Slider oldSlider = await _db.Sliders.FindAsync(Id);
         
            if (oldSlider == null) return RedirectToAction("NotFound", "AdminHome");

            SliderVM sliderVM = new SliderVM
            {
                Title = oldSlider.Title,
                SubTitle = oldSlider.SubTitle,
                Description = oldSlider.Description,
                ImageFile = oldSlider.ImageFile
            };

            return View(sliderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SliderVM sliderVM)
        {
            if (sliderVM.Id == null) return RedirectToAction("NotFound", "AdminHome");

            Slider oldSlider = _db.Sliders.Find(sliderVM.Id);

            var existsImgFile = sliderVM.ImageFile == null;
            var existsTitle = await _db.Sliders.Where(slider => slider.Title == sliderVM.Title && slider.Id != sliderVM.Id).FirstOrDefaultAsync() != null;
            var existsSubTitle = await _db.Sliders.Where(slider => slider.SubTitle == sliderVM.SubTitle && slider.Id != sliderVM.Id).FirstOrDefaultAsync() != null;
            var existsDescription = await _db.Sliders.Where(slider => slider.Description == sliderVM.Description && slider.Id != sliderVM.Id).FirstOrDefaultAsync() != null;

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
                return View(sliderVM);
            }

            if (!sliderVM.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "You can upload only images");
                return View(sliderVM);
            }
            if (sliderVM.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                return View(sliderVM);
            }

            FileManager.Delete(oldSlider.ImgUrl, _env.WebRootPath, @"\Upload\SliderImages\");
            oldSlider.ImgUrl = sliderVM.ImageFile.Upload(_env.WebRootPath, @"\Upload\SliderImages\");

            if (!ModelState.IsValid)
            {
                return View(sliderVM);
            }

            oldSlider.Title = sliderVM.Title;
            oldSlider.SubTitle = sliderVM.SubTitle;
            oldSlider.Description = sliderVM.Description;
            oldSlider.LastUpdatedDate = DateTime.Now;
            oldSlider.CreatedDate = oldSlider.CreatedDate;

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
