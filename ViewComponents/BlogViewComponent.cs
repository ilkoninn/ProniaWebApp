namespace ProniaWebApp.ViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;

        public BlogViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Blog> blogs = await _db.Blogs
                .Include(x => x.BlogImage)
                .ToListAsync();

            return View(blogs);
        }
    }
}
