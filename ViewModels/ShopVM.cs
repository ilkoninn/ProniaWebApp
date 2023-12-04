namespace ProniaWebApp.ViewModels
{
    public class ShopVM : BaseAuditableEntityVM
    {
        // Single Section
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductImage> Images { get; set; }
        public List<Tag> Tags { get; set; }

        // List Section
        public List<Product> products { get; set; }
        public List<Tag> tags { get; set; }
        public List<Category> categories { get; set; }  
    }
}
