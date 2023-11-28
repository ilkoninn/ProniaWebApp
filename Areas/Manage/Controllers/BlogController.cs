
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
            adminVM.blogs = await _db.Blogs.ToListAsync();
            adminVM.categories = await _db.Categories.ToListAsync();

            return View(adminVM);
        }


        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ICollection<Category> categories = await _db.Categories.ToListAsync();
            BlogVM BlogVM = new BlogVM
            {
                Categories = categories,
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

            _db.Blogs.Add(newBlog);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            Blog oldBlog = await _db.Blogs.FindAsync(Id);
            BlogVM blogVM = new BlogVM
            {
                Title = oldBlog.Title,
                Description = oldBlog.Description,
                CategoryId = $"{oldBlog.CategoryId}",
                Categories = await _db.Categories.ToListAsync(),
            };

            return View(blogVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(BlogVM blogVM)
        {
            Blog oldBlog = _db.Blogs.Find(blogVM.Id);

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
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Delete Section --->
        public async Task<IActionResult> Delete(int Id)
        {
            Blog Blog = await _db.Blogs.FindAsync(Id);
            _db.Blogs.Remove(Blog);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
    }
}
