using Microsoft.AspNetCore.Mvc;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CRUDSliderController : Controller
    {
        AppDbContext _db;
        public CRUDSliderController(AppDbContext appDb)
        {
            _db = appDb;
        }
        public IActionResult CreateSlider()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateSlider(Slider Slider)
        {
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
        public IActionResult UpdateSlider(Slider Slider)
        {
            Slider oldSlider = _db.Sliders.Find(Slider.Id);
            oldSlider.Title = Slider.Title;
            oldSlider.SubTitle = Slider.SubTitle;
            oldSlider.ImgUrl = Slider.ImgUrl;
            _db.SaveChanges();
            return RedirectToAction("SliderTable", "Table");
        }
        public IActionResult DeleteSlider(int Id)
        {
            Slider Slider = _db.Sliders.Find(Id);
            _db.Sliders.Remove(Slider);
            _db.SaveChanges();
            return RedirectToAction("SliderTable", "Table");
        }
    }
}
