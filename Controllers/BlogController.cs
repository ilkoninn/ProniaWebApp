
namespace ProniaWebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _db;

        public BlogController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> List()
        {
            BlogVM vm = new BlogVM();
            vm.blogs = await _db.Blogs
                .Include(x => x.BlogImage)
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .ToListAsync();

            vm.categories = await _db.Categories
                .Include(x => x.Blog)
                .ToListAsync();

            vm.tags = await _db.Tags
                .Include(x => x.Blogs)
                .ThenInclude(x => x.Blog)
                .ToListAsync();

            return View(vm);
        }

        public async Task<IActionResult> Single(int Id)
        {
            Blog singleBlog = await _db.Blogs
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .ThenInclude(pt => pt.Tag)
                .Include(x => x.BlogImage)
                .Where(c => c.Id == Id)
                .FirstOrDefaultAsync();

            if (singleBlog == null) return RedirectToAction("NotFound", "AdminHome");

            BlogVM blogVM = new BlogVM
            {
                Title = singleBlog.Title,
                Description = singleBlog.Description,
                CategoryId = $"{singleBlog.CategoryId}",
                categories = await _db.Categories
                .Include(x => x.Blog)
                .ToListAsync(),
                tags = await _db.Tags
                .Include(x => x.Blogs)
                .ToListAsync(),
                blogs = await _db.Blogs
                .Include(x => x.BlogImage)
                .ToListAsync(),
                Tags = singleBlog.Tags.Select(x => x.Tag).ToList(),
                CreatedDate = singleBlog.CreatedDate,
                UpdatedDate = singleBlog.UpdatedDate,
                Images = new List<BlogImage>()
            };

            foreach (var item in singleBlog.BlogImage)
            {
                BlogImage blogImage = new BlogImage()
                {
                    BlogId = item.BlogId,
                    ImgUrl = item.ImgUrl,
                    IsMain = item.IsMain,
                };
                blogVM.Images.Add(blogImage);
            }

            return View(blogVM);
        }
    }
}
