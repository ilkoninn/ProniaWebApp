namespace ProniaWebApp.Models
{
    public class ProductTag
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public Tag Tag { get; set; }
    }
}
