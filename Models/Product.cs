
namespace ProniaWebApp.Models
{
    public class Product : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductTag>? Tags { get; set; }
        public List<ProductImage>? ProductImage { get; set; }

    }
}
