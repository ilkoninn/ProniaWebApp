using Newtonsoft.Json;
using NuGet.Protocol;

namespace ProniaWebApp.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;

        public CartViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cookieItems = Request.Cookies["Basket"];
            List<BasketItemVM> basketItems = new();

            if (cookieItems != null)
            {
                var cookies = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(cookieItems);
                var products = await _db.Products.ToListAsync();

                foreach (var product in products)
                {
                    var cookie = cookies.FirstOrDefault(x => x.Id == product.Id);

                    if (cookie != null)
                    {
                        BasketItemVM itemVM = new()
                        {
                            Id = product.Id,
                            ImgUrl = product.ProductImage.FirstOrDefault(x => x.IsPrime == true).ImgUrl,
                            Price = product.Price,
                            TotalPrice = product.Price * cookie.Count,
                            Title = product.Title,
                            Count = cookie.Count,
                        };

                        basketItems.Add(itemVM);
                    }
                }

                return View(basketItems);
            }
            else
            {

                return View(basketItems);
            }
        }
    }
}
