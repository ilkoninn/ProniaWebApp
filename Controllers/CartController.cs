using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Net;

namespace ProniaWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext  _db;

        public CartController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var cookieItems = Request.Cookies["Basket"];
            var products = await _db.Products
                .Include(x => x.ProductImage)
                .ToListAsync();

            if(cookieItems == null && products == null) return NotFound();

            List<BasketItemVM> basketItems = new List<BasketItemVM>();

            if (cookieItems != null)
            {
                List<BasketCookieItemVM> basketCookies = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(cookieItems);

                foreach (var product in products)
                {
                    foreach (var cookie in basketCookies)
                    {
                        if (cookie.Id == product.Id)
                        {
                            BasketItemVM itemVM = new BasketItemVM()
                            {
                                Id = product.Id,
                                ImgUrl = product.ProductImage.FirstOrDefault().ImgUrl,
                                Price = product.Price,
                                TotalPrice = product.Price * cookie.Count,
                                Title = product.Title,
                                Count = cookie.Count,
                            };

                            basketItems.Add(itemVM);
                        }
                    }
                }

                return View(basketItems);
            }
            else
            {
                return View(basketItems);
            }

            
        }

        public async Task<IActionResult> AddItem(int Id)
        {
            if (Id <= 0 && Id == null) return BadRequest();
            var cookieJson = Request.Cookies["Basket"];

            

            if (cookieJson != null)
            {
                var cookies = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(cookieJson);

                foreach (var cookie in cookies)
                {
                    if(cookie.Id == Id)
                    {
                        cookie.Count += 1;
                        Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookies));
                        return RedirectToAction("Index", "Home");
                    }
                }

                BasketCookieItemVM basketCookieItemVM = new()
                {
                    Id = Id,
                    Count = 1,
                };

                cookies.Add(basketCookieItemVM);

                Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookies));
            }
            else
            {
                var cookies = new List<BasketCookieItemVM>();

                BasketCookieItemVM basketCookieItemVM = new()
                {
                    Id = Id,
                    Count = 1,
                };

                cookies.Add(basketCookieItemVM);

                Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookies));
            }


            return RedirectToAction("Index", "Home");
        }

        public ActionResult DeleteItem(int Id)
        {
            var cookiesJson = Request.Cookies["Basket"];

            List<BasketCookieItemVM> cookies;

            if (cookiesJson != null)
            {
                cookies = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(cookiesJson);

                var cookie = cookies.FirstOrDefault(c => c.Id == Id);
                if (cookie != null)
                {
                    cookies.Remove(cookie);
                }

                Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookies));
            }

            return RedirectToAction("Index");
        }
    }
}
