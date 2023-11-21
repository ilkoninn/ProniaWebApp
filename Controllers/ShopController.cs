using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ProniaWebApp.Controllers
{
    public class ShopController : Controller
    {
        AppDbContext _db;
        public ShopController(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult ShopList()
        {
            ShopVM vm = new ShopVM();
            vm.products = _db.Products
                .Include(x => x.ProductImage)
                .ToList();
            vm.tags = _db.Tags.ToList();
            vm.categories = _db.Categories.ToList();    
            return View(vm);
        }

        public IActionResult SingleProduct(int id)
        {
            ShopVM vm = new ShopVM();
            
            if(id == null) return BadRequest();
            
            Product product = _db.Products
                .Include(x => x.ProductImage)
                .Include(x => x.Category)
                .Include(x => x.ProductTags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefault(x => x.Id == id);

            if(product == null) return NotFound();

            ViewData["Pros"] = _db.Products
                .Include (x => x.ProductImage)  
                .ToList();
            
            return View(product);
        }
    }
}
