
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

            return View(adminVM);
        }


        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Categories"] = await _db.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product Product)
        {
            if (!ModelState.IsValid)
            {
                return View(Product);
            }

            _db.Products.Add(Product);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Product Product = await _db.Products.FindAsync(Id);

            return View(Product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product newProduct)
        {
            Product oldProduct = _db.Products.Find(newProduct.Id);

            if (!ModelState.IsValid)
            {
                return View();
            }

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
