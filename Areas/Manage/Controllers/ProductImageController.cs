using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Identity.Client;
using ProniaWebApp.Areas.Manage.ViewModels;
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductImageController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ProductImageController(AppDbContext appDb, IWebHostEnvironment environment)
        {
            _db = appDb;
            _env = environment;
        }

        // <--- Table Section --->
        public async Task<IActionResult> Table()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.productImages = await _db.ProductImages.ToListAsync();
            adminVM.products = await _db.Products.ToListAsync();

            return View(adminVM);
        }

        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ICollection<Product> products = await _db.Products.ToListAsync();
            ProductImageVM productImageVM = new ProductImageVM
            {
                Products = products
            };
            return View(productImageVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductImageVM productImageVM)
        {
            if (productImageVM.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image should be uploaded!");
            }
            else
            {
                if (productImageVM.ProductId == "null")
                {
                    ModelState.AddModelError("ProductId", "Please choose a product for your product image!");
                }
                else
                {
                    var newPoduct = await _db.Products
                        .Include(p => p.ProductImage)
                        .FirstOrDefaultAsync(p => p.Id == int.Parse(productImageVM.ProductId));

                    if (newPoduct == null)
                    {
                        ModelState.AddModelError("ProductId", "Invalid product selected!");
                    }
                    else
                    {
                        var primaryImageExists = newPoduct.ProductImage.FirstOrDefault(p => p.IsPrime == true && p.IsPrime == productImageVM.IsPrime);

                        if (primaryImageExists != null)
                        {
                            ModelState.AddModelError("IsPrime", "This product already has a primary image");
                        }
                    }
                }
            }

            if(ModelState.ErrorCount > 0)
            {
                productImageVM.Products = await _db.Products.ToListAsync();
                return View(productImageVM);
            }

            if (productImageVM.ImageFile != null)
            {
                if (!productImageVM.ImageFile.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                }

                if (productImageVM.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of the image is 2MB!");
                }
            }

            if (!ModelState.IsValid)
            {
                productImageVM.Products = await _db.Products.ToListAsync();
                return View(productImageVM);
            }

            var oldProduct = await _db.Products
                .Include(p => p.ProductImage)
                .FirstOrDefaultAsync(p => p.Id == int.Parse(productImageVM.ProductId));
            var existsAnyPrimaryImage = oldProduct.ProductImage.FirstOrDefault(p => p.IsPrime == true);

            if (existsAnyPrimaryImage == null)
            {
                productImageVM.IsPrime = true;
            }

            productImageVM.ImgUrl = productImageVM.ImageFile.Upload(_env.WebRootPath, @"\Upload\ProductImages\");
            ProductImage newProductImage = new ProductImage();

            newProductImage.ImgUrl = productImageVM.ImgUrl;
            newProductImage.ProductId = int.Parse(productImageVM.ProductId);
            newProductImage.IsPrime = productImageVM.IsPrime;

            _db.ProductImages.Add(newProductImage);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            ProductImage oldProductImage = await _db.ProductImages.FindAsync(Id);
            ProductImageVM productImageVM = new ProductImageVM
            {
                IsPrime = oldProductImage.IsPrime,
                ImgUrl = oldProductImage.ImgUrl,
                ProductId = $"{oldProductImage.ProductId}",
                Products = await _db.Products.ToListAsync(),
            };

            return View(productImageVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductImageVM productImageVM)
        {
            if (productImageVM.Id == null) return RedirectToAction("NotFound", "AdminHome");

            ProductImage oldProductImage = await _db.ProductImages.FindAsync(productImageVM.Id);

            if (productImageVM.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image should be uploaded!");
            }
            else
            {
                if (productImageVM.ProductId == "null")
                {
                    ModelState.AddModelError("ProductId", "Please choose a product for your product image!");
                }
                else
                {
                    var newPoduct = await _db.Products
                        .Include(p => p.ProductImage)
                        .FirstOrDefaultAsync(p => p.Id == int.Parse(productImageVM.ProductId));

                    if (newPoduct == null)
                    {
                        ModelState.AddModelError("ProductId", "Invalid product selected!");
                    }
                    else
                    {
                        var primaryImageExists = newPoduct.ProductImage.FirstOrDefault(p => p.IsPrime == true && p.IsPrime == productImageVM.IsPrime);

                        if (primaryImageExists != null)
                        {
                            ModelState.AddModelError("IsPrime", "This product already has a primary image");
                        }
                    }
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                productImageVM.Products = await _db.Products.ToListAsync();
                return View(productImageVM);
            }

            if (productImageVM.ImageFile != null)
            {
                if (!productImageVM.ImageFile.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                }

                if (productImageVM.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of the image is 2MB!");
                }
            }

            if (!ModelState.IsValid)
            {
                productImageVM.Products = await _db.Products.ToListAsync();
                return View(productImageVM);
            }

            var oldProduct = await _db.Products
                .Include(p => p.ProductImage)
                .FirstOrDefaultAsync(p => p.Id == int.Parse(productImageVM.ProductId));
            var existsAnyPrimaryImage = oldProduct.ProductImage
                .Where(p => p.IsPrime == true &&  p.Id != productImageVM.Id)
                .FirstOrDefault();

            if (existsAnyPrimaryImage == null)
            {
                productImageVM.IsPrime = true;
            }

            FileManager.Delete(oldProductImage.ImgUrl, _env.WebRootPath, @"\Upload\ProductImages\");
            productImageVM.ImgUrl = productImageVM.ImageFile.Upload(_env.WebRootPath, @"\Upload\ProductImages\");

            oldProductImage.ImgUrl = productImageVM.ImgUrl;
            oldProductImage.ProductId = int.Parse(productImageVM.ProductId);
            oldProductImage.IsPrime = productImageVM.IsPrime;

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        public async Task<IActionResult> Delete(int Id)
        {
            ProductImage productImage = await _db.ProductImages.FindAsync(Id);
            _db.ProductImages.Remove(productImage);

            var oldProduct = await _db.Products
                .Include(p => p.ProductImage)
                .FirstOrDefaultAsync(p => p.ProductImage.FirstOrDefault(p => p.Id == Id).Id == Id);

            var existsAnyPrimaryImage = oldProduct.ProductImage.FirstOrDefault(p => p.IsPrime == true);
            var existsAnyImage = oldProduct.ProductImage.FirstOrDefault();


            if (existsAnyPrimaryImage == null)
            {
                if(existsAnyImage != null)
                {
                    existsAnyImage.IsPrime = true;
                }
            }

            await _db.SaveChangesAsync();
            FileManager.Delete(productImage.ImgUrl, _env.WebRootPath, @"\Upload\ProductImages\");

            return RedirectToAction("Table");
        }
    }
}
