using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaWebApp.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public bool IsPrime { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
      

    }
}
