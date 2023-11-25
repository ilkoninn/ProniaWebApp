namespace ProniaWebApp.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Category Category { get; set; }
        public ICollection<Tag>? Tag { get; set; }
        public ICollection<BlogImage>? BlogImage { get; set; }
    }
}
