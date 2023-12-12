
using Microsoft.EntityFrameworkCore;
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public BlogController(AppDbContext appDb, IWebHostEnvironment env)
        {
            _db = appDb;
            _env = env;
        }

        // <--- Table Section --->
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Table()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.blogs = await _db.Blogs
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.BlogImage)
                .ToListAsync();
            adminVM.categories = await _db.Categories.ToListAsync();

            return View(adminVM);
        }


        // <--- Create Section --->
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ICollection<Category> categories = await _db.Categories.ToListAsync();
            ICollection<Tag> tags = await _db.Tags.ToListAsync();

            CreateBlogVM BlogVM = new CreateBlogVM
            {
                Categories = categories,
                Tags = tags,
                TagIds = new List<int> { }
            };
            return View(BlogVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM BlogVM)
        {
            var existsTitle = await _db.Blogs
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.Title == BlogVM.Title) != null;
            var existsDescription = await _db.Blogs
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.Description == BlogVM.Description) != null;

            if (BlogVM.CategoryId == "null")
            {
                ModelState.AddModelError("CategoryId", "Please, choose a category for your Blog!");
            }
            if (existsTitle)
            {
                ModelState.AddModelError("Title", "There is a same title Blog in Table!");
            }
            if (existsDescription)
            {
                ModelState.AddModelError("Description", "There is a same description Blog in Table!");
            }

            if (!ModelState.IsValid)
            {
                BlogVM.Categories = await _db.Categories.ToListAsync();
                BlogVM.Tags = await _db.Tags.ToListAsync();
                return View(BlogVM);
            }


            Blog newBlog = new Blog
            {
                Title = BlogVM.Title,
                Description = BlogVM.Description,
                CategoryId = int.Parse(BlogVM.CategoryId),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                BlogImage = new List<BlogImage>(),
            };

            // Main Image Section
            if (BlogVM.MainImage != null)
            {
                if (!BlogVM.MainImage.CheckType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                }
                if (!BlogVM.MainImage.CheckLength(2097152))
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                }

                if (!ModelState.IsValid)
                {
                    BlogVM.Categories = await _db.Categories.ToListAsync();
                    BlogVM.Tags = await _db.Tags.ToListAsync();
                    return View(BlogVM);
                }

                BlogImage BlogImage = new BlogImage
                {
                    IsMain = true,
                    Blog = newBlog,
                    ImgUrl = BlogVM.MainImage.Upload(_env.WebRootPath, @"\Upload\BlogImages\")
                };

                newBlog.BlogImage.Add(BlogImage);
            }
            else
            {
                ModelState.AddModelError("MainImage", "You must be upload a main image!");
                BlogVM.Categories = await _db.Categories.ToListAsync();
                BlogVM.Tags = await _db.Tags.ToListAsync();
                return View(BlogVM);
            }

            // Additional Images Sectionz`
            if (BlogVM.Images != null)
            {
                foreach (var item in BlogVM.Images)
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

                    BlogImage BlogImage = new BlogImage
                    {
                        IsMain = false,
                        Blog = newBlog,
                        ImgUrl = item.Upload(_env.WebRootPath, @"\Upload\BlogImages\")
                    };

                    newBlog.BlogImage.Add(BlogImage);
                }
            }

            // Blog Tag Section
            if (BlogVM.TagIds != null)
            {
                foreach (var item in BlogVM.TagIds)
                {
                    bool existsTag = await _db.Tags.AnyAsync(c => c.Id == item);

                    if (!existsTag)
                    {
                        ModelState.AddModelError("TagIds", "There is no tag like this!");
                        BlogVM.Categories = await _db.Categories.ToListAsync();
                        BlogVM.Tags = await _db.Tags.ToListAsync();
                        return View(BlogVM);
                    }

                    BlogTag BlogTag = new BlogTag()
                    {
                        Blog = newBlog,
                        TagId = item,
                    };

                    _db.BlogTags.Add(BlogTag);
                }
            }

            _db.Blogs.Add(newBlog);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }


        // <--- Update Section --->
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int Id)
        {
            Blog oldBlog = await _db.Blogs
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Tags)
                .ThenInclude(pt => pt.Tag)
                .Include(x => x.BlogImage)
                .Where(c => c.Id == Id)
                .FirstOrDefaultAsync();

            if (oldBlog == null) return RedirectToAction("NotFound", "AdminHome");

            UpdateBlogVM updateBlogVM = new UpdateBlogVM
            {
                Title = oldBlog.Title,
                Description = oldBlog.Description,
                CategoryId = $"{oldBlog.CategoryId}",
                Categories = await _db.Categories.ToListAsync(),
                Tags = await _db.Tags.ToListAsync(),
                TagIds = oldBlog.Tags.Select(pt => pt.TagId).ToList(),
                BlogImagesVM = new List<BlogImageVM>(),
            };

            foreach (var item in oldBlog.BlogImage)
            {
                BlogImageVM BlogImageVM = new BlogImageVM()
                {
                    Id = item.Id,
                    ImgUrl = item.ImgUrl,
                    IsMain = item.IsMain,
                };
                updateBlogVM.BlogImagesVM.Add(BlogImageVM);
            }

            return View(updateBlogVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateBlogVM updateBlogVM)
        {
            Blog oldBlog = await _db.Blogs
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Tags)
                .ThenInclude(pt => pt.Tag)
                .Include(x => x.BlogImage)
                .FirstOrDefaultAsync(x => x.Id == updateBlogVM.Id);
            if (oldBlog == null) return RedirectToAction("NotFound", "AdminHome");

            var existsTitle = await _db.Blogs
                .Where(x => x.IsDeleted == false)
                .Where(Blog => Blog.Title == updateBlogVM.Title && Blog.Id != updateBlogVM.Id)
                .FirstOrDefaultAsync() != null;
            var existsDescription = await _db.Blogs
                .Where(x => x.IsDeleted == false)
                .Where(Blog => Blog.Description == updateBlogVM.Description && Blog.Id != updateBlogVM.Id)
                .FirstOrDefaultAsync() != null;


            if (updateBlogVM.CategoryId == "null")
            {
                ModelState.AddModelError("CategoryId", "Please, choose a category for your Blog!");
            }
            if (existsTitle)
            {
                ModelState.AddModelError("Title", "There is a same title Blog in Table!");
            }
            if (existsDescription)
            {
                ModelState.AddModelError("Description", "There is a same description Blog in Table!");
            }

            if (!ModelState.IsValid)
            {
                return View(updateBlogVM);
            }

            oldBlog.Title = updateBlogVM.Title;
            oldBlog.Description = updateBlogVM.Description;
            oldBlog.CategoryId = int.Parse(updateBlogVM.CategoryId);
            oldBlog.UpdatedDate = DateTime.Now;
            oldBlog.CreatedDate = oldBlog.CreatedDate;

            oldBlog.Tags.Clear();

            // Blog Tag Section
            if (updateBlogVM.TagIds != null)
            {

                foreach (var item in updateBlogVM.TagIds)
                {
                    bool existsTag = await _db.Tags.AnyAsync(c => c.Id == item);

                    if (!existsTag)
                    {
                        ModelState.AddModelError("TagIds", "There is no tag like this!");
                        updateBlogVM.Categories = await _db.Categories.ToListAsync();
                        updateBlogVM.Tags = await _db.Tags.ToListAsync();
                        return View(updateBlogVM);
                    }

                    BlogTag BlogTag = new BlogTag()
                    {
                        TagId = item,
                    };

                    oldBlog.Tags.Add(BlogTag);
                }
            }

            // Main Image Section
            if (updateBlogVM.MainImage != null)
            {
                if (!updateBlogVM.MainImage.CheckType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                }
                if (!updateBlogVM.MainImage.CheckLength(2097152))
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                }

                if (!ModelState.IsValid)
                {
                    updateBlogVM.Categories = await _db.Categories.ToListAsync();
                    updateBlogVM.Tags = await _db.Tags.ToListAsync();
                    return View(updateBlogVM);
                }

                var existMainPhoto = oldBlog.BlogImage.FirstOrDefault(p => p.IsMain == true);
                existMainPhoto.ImgUrl.Delete(_env.WebRootPath, @"\Upload\BlogImages\");
                oldBlog.BlogImage.Remove(existMainPhoto);

                BlogImage BlogImage = new BlogImage
                {
                    IsMain = true,
                    Blog = oldBlog,
                    ImgUrl = updateBlogVM.MainImage.Upload(_env.WebRootPath, @"\Upload\BlogImages\")
                };

                oldBlog.BlogImage.Add(BlogImage);
            }


            if (updateBlogVM.ImageIds == null)
            {
                oldBlog.BlogImage.RemoveAll(x => x.IsMain == false);
            }
            else
            {
                List<BlogImage> removeList = oldBlog.BlogImage.Where(pt => !updateBlogVM.ImageIds.Contains(pt.Id) && pt.IsMain == null).ToList();

                if (removeList.Count > 0)
                {
                    foreach (var item in removeList)
                    {
                        oldBlog.BlogImage.Remove(item);
                        item.ImgUrl.Delete(_env.WebRootPath, @"\Upload\BlogImages\");
                    }
                }
            }

            // Additional Image Section
            TempData["Error"] = "";
            if (updateBlogVM.Images != null)
            {
                foreach (IFormFile imgFile in updateBlogVM.Images)
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

                    BlogImage BlogImages = new BlogImage()
                    {
                        IsMain = false,
                        BlogId = oldBlog.Id,
                        ImgUrl = imgFile.Upload(_env.WebRootPath, "/Upload/BlogImages/")
                    };
                    oldBlog.BlogImage.Add(BlogImages);
                }
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            Blog oldBlog = await _db.Blogs
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.BlogImage)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (oldBlog == null) return RedirectToAction("NotFound", "AdminHome");

            oldBlog.IsDeleted = true;  

            await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
