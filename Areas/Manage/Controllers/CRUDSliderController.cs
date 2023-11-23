
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CRUDSliderController : Controller
    {
        AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public CRUDSliderController(AppDbContext appDb, IWebHostEnvironment env)
        {
            _db = appDb;
            _env = env;
        }
        public IActionResult CreateSlider()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateSlider(Slider Slider)
        {

            if (!Slider.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "You can upload only images");
                return View();
            }
            if (Slider.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                return View();
            }
            
            Slider.ImgUrl = Slider.ImageFile.Upload(_env.WebRootPath, @"\Upload\SliderImages\");

            if (!ModelState.IsValid)
            {
                return View();
            }


            _db.Sliders.Add(Slider);
            _db.SaveChanges();
            return RedirectToAction("SliderTable", "Table");
        }
        public IActionResult UpdateSlider(int Id)
        {
            Slider Slider = _db.Sliders.Find(Id);
            return View(Slider);
        }
        [HttpPost]
        public IActionResult UpdateSlider(Slider newSlider)
        {
            Slider oldSlider = _db.Sliders.Find(newSlider.Id);

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

            _db.SaveChanges();
            return RedirectToAction("SliderTable", "Table");
        }
        public IActionResult DeleteSlider(int Id)
        {
            Slider Slider = _db.Sliders.Find(Id);
            _db.Sliders.Remove(Slider);
            _db.SaveChanges();
            FileManager.Delete(Slider.ImgUrl, _env.WebRootPath, @"\Upload\SliderImages\");
            return RedirectToAction("SliderTable", "Table");
        }
    }
}
