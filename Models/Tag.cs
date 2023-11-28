
namespace ProniaWebApp.Models
{
    public class Tag : BaseAuditableEntity
    {
        public string Name { get; set; }
        public ICollection<ProductTag>? Products { get; set; }
        public ICollection<BlogTag>? Blogs { get; set; }
    }
}
