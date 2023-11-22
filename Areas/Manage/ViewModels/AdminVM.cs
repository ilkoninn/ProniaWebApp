namespace ProniaWebApp.Areas.Manage.ViewModels
{
    public class AdminVM
    {
        public List<Category> categories  { get; set; }
        public List<Tag> tags { get; set; }
        public List<Product> products { get; set; }
        public List<Blog> blogs { get; set; }
        public List<Slider> slider { get; set; }
        public List<BlogImage> blogImages { get; set; }
        public List<ProductImage> productImages { get; set; }
        public List<BlogTag> blogTags { get; set; }
        public List<ProductTag> productTags { get; set; }
    }
}
