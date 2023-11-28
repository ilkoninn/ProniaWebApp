using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaWebApp.Models
{
    public class Slider : BaseAuditableEntity
    {
        public string SubTitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
