
using Microsoft.EntityFrameworkCore;
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext appDb, IWebHostEnvironment env)
        {
            _db = appDb;
            _env = env;
        }

        // <--- Table Section --->
        public async Task<IActionResult> Table()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.products = await _db.Products
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.ProductImage)
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

            CreateProductVM productVM = new CreateProductVM
            {
                Categories = categories,
                Tags = tags,
                TagIds = new List<int> { }
            };
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            var existsTitle = await _db.Products
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.Title == productVM.Title) != null;
            var existsDescription = await _db.Products
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.Description == productVM.Description) != null;
            var existsSKUCode = await _db.Products
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.SKU == productVM.SKU) != null;

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

            if (!ModelState.IsValid)
            {
                productVM.Categories = await _db.Categories.ToListAsync(); 
                productVM.Tags = await _db.Tags.ToListAsync();
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
                ProductImage = new List<ProductImage>(),
            };

            // Main Image Section
            if (productVM.MainImage != null)
            {
                if (!productVM.MainImage.CheckType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                }
                if (!productVM.MainImage.CheckLength(2097152))
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                }

                if (!ModelState.IsValid)
                {
                    productVM.Categories = await _db.Categories.ToListAsync();
                    productVM.Tags = await _db.Tags.ToListAsync();
                    return View(productVM);
                }

                ProductImage ProductImage = new ProductImage
                {
                    IsPrime = true,
                    Product = newProduct,
                    ImgUrl = productVM.MainImage.Upload(_env.WebRootPath, @"\Upload\ProductImages\")
                };

                newProduct.ProductImage.Add(ProductImage);
            }
            else
            {
                ModelState.AddModelError("MainImage", "You must be upload a main image!");
                productVM.Categories = await _db.Categories.ToListAsync();
                productVM.Tags = await _db.Tags.ToListAsync();
                return View(productVM);
            }

            // Hover Image Section
            if (productVM.HoverImage != null)
            {
                if (!productVM.HoverImage.CheckType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                }
                if (!productVM.HoverImage.CheckLength(2097152))
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                }

                if (!ModelState.IsValid)
                {
                    productVM.Categories = await _db.Categories.ToListAsync();
                    productVM.Tags = await _db.Tags.ToListAsync();
                    return View(productVM);
                }

                ProductImage ProductImage = new ProductImage
                {
                    IsPrime = false,
                    Product = newProduct,
                    ImgUrl = productVM.HoverImage.Upload(_env.WebRootPath, @"\Upload\ProductImages\")
                };

                newProduct.ProductImage.Add(ProductImage);
            }
            else
            {
                ModelState.AddModelError("HoverImage", "You must be upload a hover image!");
                productVM.Categories = await _db.Categories.ToListAsync();
                productVM.Tags = await _db.Tags.ToListAsync();
                return View(productVM);
            }

            // Additional Images Sectionz`
            if (productVM.Images != null)
            {
                foreach (var item in productVM.Images)
                {
                    if (!item.CheckType("image/"))
                    {
                        TempData["Error"] += $"{item.FileName} is not image type!\n";
                        continue;
                    }
                    if (!item.CheckLength(2097152))
                    {
                        TempData["Error"] += $"{item.FileName} size is must lower than 2MB!";
                        continue;
                    }

                    ProductImage ProductImage = new ProductImage
                    {
                        IsPrime = null,
                        Product = newProduct,
                        ImgUrl = item.Upload(_env.WebRootPath, @"\Upload\ProductImages\")
                    };

                    newProduct.ProductImage.Add(ProductImage);
                }
            }

            // Product Tag Section
            if (productVM.TagIds != null)
            {
                foreach (var item in productVM.TagIds)
                {
                    bool existsTag = await _db.Tags.AnyAsync(c => c.Id == item);

                    if (!existsTag)
                    {
                        ModelState.AddModelError("TagIds", "There is no tag like this!");
                        productVM.Categories = await _db.Categories.ToListAsync();
                        productVM.Tags = await _db.Tags.ToListAsync();
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
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Tags)
                .ThenInclude(pt => pt.Tag)
                .Include(x => x.ProductImage)
                .Where(c => c.Id == Id)
                .FirstOrDefaultAsync();

            if (oldProduct == null) return RedirectToAction("NotFound", "AdminHome");

            UpdateProductVM updateProductVM = new UpdateProductVM
            {
                Title = oldProduct.Title,
                Description = oldProduct.Description,
                Price = oldProduct.Price,
                SKU = oldProduct.SKU,
                CategoryId = $"{oldProduct.CategoryId}",
                Categories = await _db.Categories.ToListAsync(),
                Tags = await _db.Tags.ToListAsync(),
                TagIds = oldProduct.Tags.Select(pt => pt.TagId).ToList(),
                ProductImagesVM = new List<ProductImageVM>(),
            };

            foreach (var item in oldProduct.ProductImage)
            {
                ProductImageVM productImageVM = new ProductImageVM()
                {
                    Id = item.Id,
                    ImgUrl = item.ImgUrl,
                    IsPrime = item.IsPrime,
                };
                updateProductVM.ProductImagesVM.Add(productImageVM);
            }

            return View(updateProductVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM updateProductVM)
        {
            Product oldProduct = await _db.Products
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Tags)
                .ThenInclude(pt => pt.Tag)
                .Include(x => x.ProductImage)
                .FirstOrDefaultAsync(x => x.Id == updateProductVM.Id);
            if (oldProduct == null) return RedirectToAction("NotFound", "AdminHome");

            var existsTitle = await _db.Products
                .Where(x => x.IsDeleted == false)
                .Where(product => product.Title == updateProductVM.Title && product.Id != updateProductVM.Id)
                .FirstOrDefaultAsync() != null;
            var existsDescription = await _db.Products
                .Where(x => x.IsDeleted == false)
                .Where(product => product.Description == updateProductVM.Description && product.Id != updateProductVM.Id)
                .FirstOrDefaultAsync() != null;
            var existsSKUCode = await _db.Products
                .Where(x => x.IsDeleted == false)
                .Where(product => product.SKU == updateProductVM.SKU && product.Id != updateProductVM.Id)
                .FirstOrDefaultAsync() != null;


            if (updateProductVM.CategoryId == "null")
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

            if (!ModelState.IsValid)
            {
                return View(updateProductVM);
            }

            oldProduct.Title = updateProductVM.Title;
            oldProduct.Description = updateProductVM.Description;
            oldProduct.Price = updateProductVM.Price;
            oldProduct.SKU = updateProductVM.SKU;
            oldProduct.CategoryId = int.Parse(updateProductVM.CategoryId);
            oldProduct.UpdatedDate = DateTime.Now;
            oldProduct.CreatedDate = oldProduct.CreatedDate;

            oldProduct.Tags.Clear();

            // Product Tag Section
            if (updateProductVM.TagIds != null)
            {

                foreach (var item in updateProductVM.TagIds)
                {
                    bool existsTag = await _db.Tags.AnyAsync(c => c.Id == item);

                    if (!existsTag)
                    {
                        ModelState.AddModelError("TagIds", "There is no tag like this!");
                        updateProductVM.Categories = await _db.Categories.ToListAsync();
                        updateProductVM.Tags = await _db.Tags.ToListAsync();
                        return View(updateProductVM);
                    }

                    ProductTag productTag = new ProductTag()
                    {
                        TagId = item,
                    };

                    oldProduct.Tags.Add(productTag);
                }
            }

            // Main Image Section
            if (updateProductVM.MainImage != null)
            {
                if (!updateProductVM.MainImage.CheckType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                }
                if (!updateProductVM.MainImage.CheckLength(2097152))
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                }

                if (!ModelState.IsValid)
                {
                    updateProductVM.Categories = await _db.Categories.ToListAsync();
                    updateProductVM.Tags = await _db.Tags.ToListAsync();
                    return View(updateProductVM);
                }

                var existMainPhoto = oldProduct.ProductImage.FirstOrDefault(p => p.IsPrime == true);
                existMainPhoto.ImgUrl.Delete(_env.WebRootPath, @"\Upload\ProductImages\");
                oldProduct.ProductImage.Remove(existMainPhoto);

                ProductImage ProductImage = new ProductImage
                {
                    IsPrime = true,
                    Product = oldProduct,
                    ImgUrl = updateProductVM.MainImage.Upload(_env.WebRootPath, @"\Upload\ProductImages\")
                };

                oldProduct.ProductImage.Add(ProductImage);
            }

            // Hover Image Section
            if (updateProductVM.HoverImage != null)
            {
                if (!updateProductVM.HoverImage.CheckType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                }
                if (!updateProductVM.HoverImage.CheckLength(2097152))
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                }

                if (!ModelState.IsValid)
                {
                    updateProductVM.Categories = await _db.Categories.ToListAsync();
                    updateProductVM.Tags = await _db.Tags.ToListAsync();
                    return View(updateProductVM);
                }

                var existMainPhoto = oldProduct.ProductImage.FirstOrDefault(p => p.IsPrime == false);
                existMainPhoto.ImgUrl.Delete(_env.WebRootPath, @"\Upload\ProductImages\");
                oldProduct.ProductImage.Remove(existMainPhoto);

                ProductImage ProductImage = new ProductImage
                {
                    IsPrime = false,
                    Product = oldProduct,
                    ImgUrl = updateProductVM.HoverImage.Upload(_env.WebRootPath, @"\Upload\ProductImages\")
                };

                oldProduct.ProductImage.Add(ProductImage);
            }

            if (updateProductVM.ImageIds == null)
            {
                oldProduct.ProductImage.RemoveAll(x => x.IsPrime == null);
            }
            else
            {
                List<ProductImage> removeList = oldProduct.ProductImage.Where(pt => !updateProductVM.ImageIds.Contains(pt.Id) && pt.IsPrime == null).ToList();

                if (removeList.Count > 0)
                {
                    foreach (var item in removeList)
                    {
                        oldProduct.ProductImage.Remove(item);
                        item.ImgUrl.Delete(_env.WebRootPath, @"\Upload\ProductImages\");
                    }
                }
            }

            // Additional Image Section
            TempData["Error"] = "";
            if (updateProductVM.Images != null)
            {
                foreach (IFormFile imgFile in updateProductVM.Images)
                {
                    if (!imgFile.CheckType("image/"))
                    {
                        TempData["Error"] += $"{imgFile.FileName} is not image type!\n";
                        continue;
                    }
                    if (!imgFile.CheckLength(2097152))
                    {
                        TempData["Error"] += $"{imgFile.FileName} size is must lower than 2MB!";
                        continue;
                    }

                    ProductImage ProductImages = new ProductImage()
                    {
                        IsPrime = null,
                        ProductId = oldProduct.Id,
                        ImgUrl = imgFile.Upload(_env.WebRootPath, "/Upload/ProductImages/")
                    };
                    oldProduct.ProductImage.Add(ProductImages);
                }
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        public async Task<IActionResult> Delete(int Id)
        {
            Product oldProduct = await _db.Products
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.ProductImage)
                .FirstOrDefaultAsync(x => x.Id == Id);
            if (oldProduct == null) return RedirectToAction("NotFound", "AdminHome");

            oldProduct.IsDeleted = true;
            
            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
