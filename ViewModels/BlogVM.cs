namespace ProniaWebApp.ViewModels
{
    public class BlogVM : BaseAuditableEntityAdminVM
    {
        // Single Section
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public List<BlogImage> Images { get; set; }
        public List<Tag> Tags { get; set; }

        // List Section
        public List<Blog> blogs { get; set; }
        public List<Tag> tags { get; set; }
        public List<Category> categories { get; set; }
    }
}
