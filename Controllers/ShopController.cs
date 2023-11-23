using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ProniaWebApp.Controllers
{
    public class ShopController : Controller
    {
        AppDbContext _db;
        public ShopController(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult ShopList()
        {   
            return View();
        }

        public IActionResult SingleProduct(int id)
        {   
            return View();
        }
    }
}
