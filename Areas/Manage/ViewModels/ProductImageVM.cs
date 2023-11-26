using System.Drawing;

namespace ProniaWebApp.Areas.Manage.ViewModels
{
    public class ProductImageVM
    {
        public int Id { get; set; }
        public bool IsPrime { get; set; }
        public string? ImgUrl { get; set; }
        public string ProductId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
