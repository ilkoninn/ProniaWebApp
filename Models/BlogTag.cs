namespace ProniaWebApp.Models
{
    public class BlogTag
    {
        public int Id { get; set; }
        public Blog Blog { get; set; }
        public Tag Tag { get; set; }
    }
}
