
namespace ProniaWebApp.Areas.Manage.ViewModels
{
    public class ProductImageVM : BaseAuditableEntityVM
    {
        public bool IsPrime { get; set; }
        public string? ImgUrl { get; set; }
        public string ProductId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
