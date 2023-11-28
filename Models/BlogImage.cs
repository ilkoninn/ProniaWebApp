using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaWebApp.Models
{
    public class BlogImage : BaseAuditableEntity
    {
        public string ImgUrl { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
