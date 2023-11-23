namespace ProniaWebApp.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Category Category { get; set; }
        public List<Tag>? Tag { get; set; }
        public List<BlogImage>? BlogImage { get; set; }
    }
}
