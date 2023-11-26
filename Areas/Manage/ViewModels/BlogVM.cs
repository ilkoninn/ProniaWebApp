namespace ProniaWebApp.Areas.Manage.ViewModels
{
    public class BlogVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public ICollection<Category>? Categories { get; set; }
    }
}
