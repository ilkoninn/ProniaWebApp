using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> List()
        {   
            ShopVM vm = new ShopVM();
            vm.products = await _db.Products
                .Include(x => x.ProductImage)
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .ToListAsync();

            vm.categories = await _db.Categories
                .Include(x => x.Product)
                .ToListAsync();

            vm.tags = await _db.Tags
                .Include(x => x.Products)
                .ThenInclude(x => x.Product)
                .ToListAsync();

            return View(vm);
        }

        public IActionResult Single(int Id)
        {

            return View();
        }
    }
}
