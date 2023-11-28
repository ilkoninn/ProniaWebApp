

using Microsoft.EntityFrameworkCore;
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        public ProductController(AppDbContext appDb, IWebHostEnvironment env)
        {
            _db = appDb;
        }

        // <--- Table Section --->
        public async Task<IActionResult> Table()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.products = await _db.Products
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .ToListAsync();
            adminVM.categories = await _db.Categories.ToListAsync();

            return View(adminVM);
        }


        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ICollection<Category> categories = await _db.Categories.ToListAsync();
            ICollection<Tag> tags = await _db.Tags.ToListAsync();

            ProductVM productVM = new ProductVM 
            {
                Categories = categories,
                Tags = tags,
            };  
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM productVM)
        {
            var existsTitle = await _db.Products.FirstOrDefaultAsync(x => x.Title == productVM.Title) != null;
            var existsDescription = await _db.Products.FirstOrDefaultAsync(x => x.Description == productVM.Description) != null;
            var existsSKUCode = await _db.Products.FirstOrDefaultAsync(x => x.SKU == productVM.SKU) != null;


            if(productVM.CategoryId == "null")
            {
                ModelState.AddModelError("CategoryId", "Please, choose a category for your product!");
            }
            if (existsTitle)
            {
                ModelState.AddModelError("Title", "There is a same title product in Table!");
            }
            if (existsDescription)
            {
                ModelState.AddModelError("Description", "There is a same description product in Table!");
            }
            if (existsSKUCode)
            {
                ModelState.AddModelError("SKU", "There is a same SKU code product in Table!");
            }

            if (ModelState.ErrorCount > 0)
            {
                productVM.Categories = await _db.Categories.ToListAsync();
                return View(productVM);
            }

            if (!ModelState.IsValid)
            {
                productVM.Categories = await _db.Categories.ToListAsync();
                return View(productVM);
            }

            Product newProduct = new Product
            {
                Title = productVM.Title,
                Description = productVM.Description,
                Price = productVM.Price,
                SKU = productVM.SKU,
                CategoryId = int.Parse(productVM.CategoryId),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            if(productVM.TagIds != null)
            {
                foreach (var item in productVM.TagIds)
                {
                    bool existsTag = await _db.Tags.AnyAsync(c => c.Id == item);

                    if (!existsTag)
                    {
                        ModelState.AddModelError("TagIds", "There is no tag like this!");
                        return View(productVM);
                    }

                    ProductTag productTag = new ProductTag()
                    {
                        Product = newProduct,
                        TagId = item,
                    };

                    _db.ProductTags.Add(productTag);
                }
            }

            _db.Products.Add(newProduct);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            Product oldProduct = await _db.Products
                .Include(x => x.Tags)
                .ThenInclude(pt => pt.Tag)
                .Where(c => c.Id == Id)
                .FirstOrDefaultAsync();

            if (oldProduct == null) return RedirectToAction("NotFound", "AdminHome");

            ProductVM productVM = new ProductVM
            {
                Title = oldProduct.Title,
                Description = oldProduct.Description,
                Price = oldProduct.Price,
                SKU = oldProduct.SKU,
                CategoryId = $"{oldProduct.CategoryId}",
                Categories = await _db.Categories.ToListAsync(),
                Tags = await _db.Tags.ToListAsync(),
                TagIds = oldProduct.Tags.Select(pt => pt.TagId).ToList(),
            };

            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductVM productVM)
        {
            Product oldProduct = await _db.Products
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == productVM.Id);
            if (oldProduct == null) return RedirectToAction("NotFound", "AdminHome");

            var existsTitle = await _db.Products.Where(product => product.Title == productVM.Title && product.Id != productVM.Id).FirstOrDefaultAsync() != null;
            var existsDescription = await _db.Products.Where(product => product.Description == productVM.Description && product.Id != productVM.Id).FirstOrDefaultAsync() != null;
            var existsSKUCode = await _db.Products.Where(product => product.SKU == productVM.SKU && product.Id != productVM.Id).FirstOrDefaultAsync() != null;


            if (productVM.CategoryId == "null")
            {
                ModelState.AddModelError("CategoryId", "Please, choose a category for your product!");
            }
            if (existsTitle)
            {
                ModelState.AddModelError("Title", "There is a same title product in Table!");
            }
            if (existsDescription)
            {
                ModelState.AddModelError("Description", "There is a same description product in Table!");
            }
            if (existsSKUCode)
            {
                ModelState.AddModelError("SKU", "There is a same SKU code product in Table!");
            }

            //if (ModelState.ErrorCount > 0)
            //{
            //    productVM.Categories = await _db.Categories.ToListAsync();
            //    return View(productVM);
            //}

            if (!ModelState.IsValid)
            {
                productVM.Categories = await _db.Categories.ToListAsync();
                return View(productVM);
            }

            oldProduct.Title = productVM.Title;
            oldProduct.Description = productVM.Description;
            oldProduct.Price = productVM.Price;
            oldProduct.SKU = productVM.SKU;
            oldProduct.CategoryId = int.Parse(productVM.CategoryId);
            oldProduct.UpdatedDate = DateTime.Now;
            oldProduct.CreatedDate = oldProduct.CreatedDate;

            oldProduct.Tags.Clear();

            if (productVM.TagIds != null)
            {

                foreach (var item in productVM.TagIds)
                {
                    bool existsTag = await _db.Tags.AnyAsync(c => c.Id == item);

                    if (!existsTag)
                    {
                        ModelState.AddModelError("TagIds", "There is no tag like this!");
                        return View(productVM);
                    }

                    ProductTag productTag = new ProductTag()
                    {
                        TagId = item,
                    };

                    oldProduct.Tags.Add(productTag);
                }
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        public async Task<IActionResult> Delete(int Id)
        {
            Product oldProduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == Id);
            if (oldProduct == null) return RedirectToAction("NotFound", "AdminHome");

            _db.Products.Remove(oldProduct);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
    }
}
