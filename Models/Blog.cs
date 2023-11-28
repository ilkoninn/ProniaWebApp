
namespace ProniaWebApp.Models
{
    public class Blog : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<BlogTag>? Tags { get; set; }
        public ICollection<BlogImage>? BlogImage { get; set; }
    }
}
