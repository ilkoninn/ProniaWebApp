
using ProniaWebApp.Areas.Manage.ViewModels;
using ProniaWebApp.Models;

namespace ProniaWebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _db;
        public BlogController(AppDbContext appDb, IWebHostEnvironment env)
        {
            _db = appDb;
        }

        // <--- Table Section --->
        public async Task<IActionResult> Table()
        {
            AdminVM adminVM = new AdminVM();
            adminVM.blogs = await _db.Blogs
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
            BlogVM BlogVM = new BlogVM
            {
                Categories = categories,
                Tags = tags,
            };
            return View(BlogVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogVM blogVM)
        {
            var existsTitle = await _db.Blogs.FirstOrDefaultAsync(x => x.Title == blogVM.Title) != null;
            var existsDescription = await _db.Blogs.FirstOrDefaultAsync(x => x.Description == blogVM.Description) != null;


            if (blogVM.CategoryId == "null")
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

            if (ModelState.ErrorCount > 0)
            {
                blogVM.Categories = await _db.Categories.ToListAsync();
                return View(blogVM);
            }

            if (!ModelState.IsValid)
            {
                blogVM.Categories = await _db.Categories.ToListAsync();
                return View(blogVM);
            }

            Blog newBlog = new Blog
            {
                Title = blogVM.Title,
                Description = blogVM.Description,
                CategoryId = int.Parse(blogVM.CategoryId),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            if (blogVM.TagIds != null)
            {
                foreach (var item in blogVM.TagIds)
                {
                    bool existsTag = await _db.Tags.AnyAsync(c => c.Id == item);

                    if (!existsTag)
                    {
                        ModelState.AddModelError("TagIds", "There is no tag like this!");
                        return View(blogVM);
                    }

                    BlogTag blogTag = new BlogTag()
                    {
                        Blog = newBlog,
                        TagId = item,
                    };

                    _db.BlogTags.Add(blogTag);
                }
            }

            _db.Blogs.Add(newBlog);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {

            Blog oldBlog = await _db.Blogs
                .Include(x => x.Tags)
                .ThenInclude(pt => pt.Tag)
                .Where(c => c.Id == Id) 
                .FirstOrDefaultAsync(); ;
            if (oldBlog == null) return RedirectToAction("NotFound", "AdminHome"); 


            BlogVM blogVM = new BlogVM
            {
                Title = oldBlog.Title,
                Description = oldBlog.Description,
                CategoryId = $"{oldBlog.CategoryId}",
                Categories = await _db.Categories.ToListAsync(),
                Tags = await _db.Tags.ToListAsync(),
                TagIds = oldBlog.Tags.Select(pt => pt.TagId).ToList(),
            };

            return View(blogVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BlogVM blogVM)
        {
            Blog oldBlog = await _db.Blogs
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == blogVM.Id);

            if (oldBlog == null) return RedirectToAction("NotFound", "AdminHome");

            var existsTitle = await _db.Blogs
                .Where(Blog => Blog.Title == blogVM.Title && Blog.Id != blogVM.Id)
                .FirstOrDefaultAsync() != null;
            var existsDescription = await _db.Blogs
                .Where(Blog => Blog.Description == blogVM.Description && Blog.Id != blogVM.Id)
                .FirstOrDefaultAsync() != null;


            if (blogVM.CategoryId == "null")
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

            if (ModelState.ErrorCount > 0)
            {
                blogVM.Categories = await _db.Categories.ToListAsync();
                return View(blogVM);
            }

            if (!ModelState.IsValid)
            {
                blogVM.Categories = await _db.Categories.ToListAsync();
                return View(blogVM);
            }

            oldBlog.Title = blogVM.Title;
            oldBlog.Description = blogVM.Description;
            oldBlog.CategoryId = int.Parse(blogVM.CategoryId);
            oldBlog.UpdatedDate = DateTime.Now;
            oldBlog.CreatedDate = oldBlog.CreatedDate;

            oldBlog.Tags.Clear();

            if (blogVM.TagIds != null)
            {

                foreach (var item in blogVM.TagIds)
                {
                    bool existsTag = await _db.Tags.AnyAsync(c => c.Id == item);

                    if (!existsTag)
                    {
                        ModelState.AddModelError("TagIds", "There is no tag like this!");
                        return View(blogVM);
                    }

                    BlogTag blogTag = new BlogTag()
                    {
                        Blog = oldBlog,
                        TagId = item,
                    };

                    _db.BlogTags.Add(blogTag);
                }
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        public async Task<IActionResult> Delete(int Id)
        {
            Blog Blog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == Id);
            if (Blog == null) return RedirectToAction("NotFound", "AdminHome");

            _db.Blogs.Remove(Blog);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
    }
}
