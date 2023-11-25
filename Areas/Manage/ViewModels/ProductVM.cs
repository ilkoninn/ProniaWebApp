namespace ProniaWebApp.Areas.Manage.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string CategoryId { get; set; }
        public ICollection<Category>? Categories { get; set; }

    }
}
