
namespace ProniaWebApp.Areas.Manage.ViewModels
{
    public class BlogImageVM : BaseAuditableEntityVM
    {
        public string? ImgUrl { get; set; }
        public string BlogId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public ICollection<Blog>? Blogs { get; set; }
    }
}
