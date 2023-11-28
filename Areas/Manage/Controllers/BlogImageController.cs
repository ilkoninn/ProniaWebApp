

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BlogImageController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public BlogImageController(AppDbContext appDb, IWebHostEnvironment environment)
        {
            _db = appDb;
            _env = environment;
        }

        // <--- Table Section --->
        public async Task<IActionResult> Table()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.blogImages = await _db.BlogsImages.ToListAsync();
            adminVM.blogs = await _db.Blogs.ToListAsync();

            return View(adminVM);
        }

        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ICollection<Blog> Blogs = await _db.Blogs.ToListAsync();
            BlogImageVM blogImageVM = new BlogImageVM
            {
                Blogs = Blogs
            };
            return View(blogImageVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogImageVM blogImageVM)
        {
            if (blogImageVM.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image should be uploaded!");
            }
            if (blogImageVM.BlogId == "null")
            {
                ModelState.AddModelError("BlogId", "Please choose a Blog for your Blog image!");
            }

            if (ModelState.ErrorCount > 0)
            {
                blogImageVM.Blogs = await _db.Blogs.ToListAsync();
                return View(blogImageVM);
            }

            if (blogImageVM.ImageFile != null)
            {
                if (!blogImageVM.ImageFile.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                }

                if (blogImageVM.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of the image is 2MB!");
                }
            }

            if (!ModelState.IsValid)
            {
                blogImageVM.Blogs = await _db.Blogs.ToListAsync();
                return View(blogImageVM);
            }


            blogImageVM.ImgUrl = blogImageVM.ImageFile.Upload(_env.WebRootPath, @"\Upload\BlogImages\");
            BlogImage newBlogImage = new BlogImage
            {
                ImgUrl = blogImageVM.ImgUrl,
                BlogId = int.Parse(blogImageVM.BlogId),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            _db.BlogsImages.Add(newBlogImage);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            BlogImage oldBlogImage = await _db.BlogsImages.FirstOrDefaultAsync(x => x.Id == Id);
            if (oldBlogImage == null) return RedirectToAction("NotFound", "AdminHome");

            BlogImageVM BlogImageVM = new BlogImageVM
            {
                ImageFile = oldBlogImage.ImageFile,
                BlogId = $"{oldBlogImage.BlogId}",
                Blogs = await _db.Blogs.ToListAsync(),
            };

            return View(BlogImageVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BlogImageVM BlogImageVM)
        {
            BlogImage oldBlogImage = await _db.BlogsImages.FirstOrDefaultAsync(x => x.Id == BlogImageVM.Id);
            if (oldBlogImage == null) return RedirectToAction("NotFound", "AdminHome");

            if (BlogImageVM.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image should be uploaded!");
            }
            if (BlogImageVM.BlogId == "null")
            {
                ModelState.AddModelError("BlogId", "Please choose a Blog for your Blog image!");
            }

            if (ModelState.ErrorCount > 0)
            {
                BlogImageVM.Blogs = await _db.Blogs.ToListAsync();
                return View(BlogImageVM);
            }

            if (!BlogImageVM.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "You can upload only images");
            }
            if (BlogImageVM.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The maximum size of the image is 2MB!");
            }

            if (!ModelState.IsValid)
            {
                BlogImageVM.Blogs = await _db.Blogs.ToListAsync();
                return View(BlogImageVM);
            }

            FileManager.Delete(oldBlogImage.ImgUrl, _env.WebRootPath, @"\Upload\BlogImages\");
            BlogImageVM.ImgUrl = BlogImageVM.ImageFile.Upload(_env.WebRootPath, @"\Upload\BlogImages\");

            oldBlogImage.ImgUrl = BlogImageVM.ImgUrl;
            oldBlogImage.BlogId = int.Parse(BlogImageVM.BlogId);
            oldBlogImage.UpdatedDate = DateTime.Now;
            oldBlogImage.CreatedDate = oldBlogImage.CreatedDate;

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        public async Task<IActionResult> Delete(int Id)
        {
            BlogImage oldBlogImage = await _db.BlogsImages.FirstOrDefaultAsync(x => x.Id == Id);
            if (oldBlogImage == null) return RedirectToAction("NotFound", "AdminHome");
            
            _db.BlogsImages.Remove(oldBlogImage);
            FileManager.Delete(oldBlogImage.ImgUrl, _env.WebRootPath, @"\Upload\BlogImages\");

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
    }
}
