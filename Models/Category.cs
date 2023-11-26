namespace ProniaWebApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public ICollection<Product>? Product { get; set; }
        public ICollection<Blog>? Blog { get; set; }

    }
}
