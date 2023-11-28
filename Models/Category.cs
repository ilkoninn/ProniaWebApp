namespace ProniaWebApp.Models
{
    public class Category : BaseAuditableEntity
    {
        public string Name { get; set; }
        public ICollection<Product>? Product { get; set; }
        public ICollection<Blog>? Blog { get; set; }
        
    }
}
