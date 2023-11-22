using Microsoft.AspNetCore.Mvc;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CRUDProductController : Controller
    {
        AppDbContext _db;
        public CRUDProductController(AppDbContext appDb)
        {
            _db = appDb;
        }
        public IActionResult CreateProduct()
        {
            ViewData["Categories"] = _db.Categories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(Product Product)
        {
            _db.Products.Add(Product);
            _db.SaveChanges();
            return RedirectToAction("ProductTable", "Table");
        }
        public IActionResult UpdateProduct(int Id)
        {
            Product Product = _db.Products.Find(Id);
            return View(Product);
        }
        [HttpPost]
        public IActionResult UpdateProduct(Product Product)
        {
            Product oldProduct = _db.Products.Find(Product.Id);
            _db.SaveChanges();
            return RedirectToAction("ProductTable", "Table");
        }
        public IActionResult DeleteProduct(int Id)
        {
            Product Product = _db.Products.Find(Id);
            _db.Products.Remove(Product);
            _db.SaveChanges();
            return RedirectToAction("ProductTable", "Table");
        }
    }
}
