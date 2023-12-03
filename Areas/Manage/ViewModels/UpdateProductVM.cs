
namespace ProniaWebApp.Areas.Manage.ViewModels
{
    public class UpdateProductVM : BaseAuditableEntityAdminVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string CategoryId { get; set; }
        public List<int>? TagIds { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public ICollection<Tag>? Tags { get; set; }

        // Product Image Fields
        public IFormFile? MainImage { get; set; }
        public IFormFile? HoverImage { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<ProductImageVM>? ProductImagesVM { get; set; }
        public List<int>? ImageIds { get; set; }
    }
    public class ProductImageVM : BaseAuditableEntityAdminVM
    {
        public string ImgUrl { get; set; }
        public bool? IsPrime { get; set; }
    }
}
