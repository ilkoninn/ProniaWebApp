
using ProniaWebApp.Areas.Manage.ViewModels;
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
            adminVM.products = await _db.Products.ToListAsync();
            adminVM.categories = await _db.Categories.ToListAsync();

            return View(adminVM);
        }


        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ICollection<Category> categories = await _db.Categories.ToListAsync();
            ProductVM productVM = new ProductVM 
            {
                Categories = categories,
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
                LastUpdatedDate = DateTime.Now,
            };

            _db.Products.Add(newProduct);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            Product oldProduct = await _db.Products.FindAsync(Id);
            ProductVM productVM = new ProductVM
            {
                Title = oldProduct.Title,
                Description = oldProduct.Description,
                Price = oldProduct.Price,
                SKU = oldProduct.SKU,
                CategoryId = $"{oldProduct.CategoryId}",
                Categories = await _db.Categories.ToListAsync(),
            };

            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductVM productVM)
        {
            Product oldProduct = _db.Products.Find(productVM.Id);

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
            oldProduct.LastUpdatedDate = DateTime.Now;
            oldProduct.CreatedDate = oldProduct.CreatedDate;

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        public async Task<IActionResult> Delete(int Id)
        {
            Product Product = await _db.Products.FindAsync(Id);
            _db.Products.Remove(Product);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
    }
}
