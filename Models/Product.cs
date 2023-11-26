namespace ProniaWebApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Tag>? Tag { get; set; }
        public List<ProductImage>? ProductImage { get; set; }

    }
}
