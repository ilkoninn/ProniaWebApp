
namespace ProniaWebApp.Models
{
    public class BlogImage : BaseAuditableEntity
    {
        public string ImgUrl { get; set; }
        public bool IsMain { get; set; }
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
    }
}
