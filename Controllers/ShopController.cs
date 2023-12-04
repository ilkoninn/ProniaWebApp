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

        public async Task<IActionResult> Single(int Id)
        {
            Product singleProduct = await _db.Products
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .ThenInclude(pt => pt.Tag)
                .Include(x => x.ProductImage)
                .Where(c => c.Id == Id)
                .FirstOrDefaultAsync();

            if (singleProduct == null) return RedirectToAction("NotFound", "AdminHome");

            ShopVM shopVM = new ShopVM
            {
                Id = singleProduct.Id,
                Title = singleProduct.Title,
                Description = singleProduct.Description,
                Category = singleProduct.Category,
                Price = singleProduct.Price,
                SKU = singleProduct.SKU,
                tags = await _db.Tags
                .Include(x => x.Products)
                .ToListAsync(),
                products = await _db.Products
                .Include(x => x.Category)
                .Include(x => x.ProductImage)
                .ToListAsync(),
                Tags = singleProduct.Tags.Select(x => x.Tag).ToList(),
                CreatedDate = singleProduct.CreatedDate,
                UpdatedDate = singleProduct.UpdatedDate,
                Images = new List<ProductImage>()
            };

            foreach (var item in singleProduct.ProductImage)
            {
                ProductImage blogImage = new ProductImage()
                {
                    ProductId = item.ProductId,
                    ImgUrl = item.ImgUrl,
                    IsPrime = item.IsPrime,
                };
                shopVM.Images.Add(blogImage);
            }

            return View(shopVM);
        }
    }
}
