namespace ProniaWebApp.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;

        public ProductViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Product> products = await _db.Products
                .Include(x => x.Category)
                .Include(x => x.ProductImage)
                .ToListAsync();
                
            return View(products);
        }
    }
}
