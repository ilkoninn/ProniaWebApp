
namespace ProniaWebApp.Models
{
    public class ProductImage : BaseAuditableEntity
    {
        public string ImgUrl { get; set; }
        public bool? IsPrime { get; set; }
        public bool IsDeleted { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
