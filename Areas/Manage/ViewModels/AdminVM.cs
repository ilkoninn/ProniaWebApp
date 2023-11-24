namespace ProniaWebApp.Areas.Manage.ViewModels
{
    public class AdminVM
    {
        public ICollection<Category> categories { get; set; }
        public ICollection<Tag> tags { get; set; }
        public ICollection<Product> products { get; set; }
        public ICollection<Blog> blogs { get; set; }
        public ICollection<Slider> sliders { get; set; }
        public ICollection<BlogImage> blogImages { get; set; }
        public ICollection<ProductImage> productImages { get; set; }
    }
}
