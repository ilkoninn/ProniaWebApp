namespace ProniaWebApp.Models
{
    public class BlogImage
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Blog Blog { get; set; }
    }
}
