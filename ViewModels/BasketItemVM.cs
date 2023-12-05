namespace ProniaWebApp.ViewModels
{
    public class BasketItemVM : BaseAuditableEntityVM
    {
        public string ImgUrl { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
