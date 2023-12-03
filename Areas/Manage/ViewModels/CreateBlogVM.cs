namespace ProniaWebApp.Areas.Manage.ViewModels
{
    public class CreateBlogVM: BaseAuditableEntityAdminVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public List<int>? TagIds { get; set; }
        public ICollection<Tag>? Tags { get; set; }

        // Blog Image Fields
        [Required]
        public IFormFile MainImage { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
