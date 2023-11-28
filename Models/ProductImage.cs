using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaWebApp.Models
{
    public class ProductImage : BaseAuditableEntity
    {
        public string ImgUrl { get; set; }
        public bool IsPrime { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
      

    }
}
