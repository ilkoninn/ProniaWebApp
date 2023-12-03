namespace ProniaWebApp.Areas.Manage.ViewModels
{
    public class UpdateBlogVM: BaseAuditableEntityAdminVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public List<int>? TagIds { get; set; }
        public ICollection<Tag>? Tags { get; set; }

        // Blog Image Fields
        public IFormFile? MainImage { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<BlogImageVM>? BlogImagesVM { get; set; }
        public List<int>? ImageIds { get; set; }
    }
    public class BlogImageVM : BaseAuditableEntityAdminVM
    {
        public string ImgUrl { get; set; }
        public bool? IsMain { get; set; }
    }
}
