

namespace ProniaWebApp.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Slider> Sliders { get; set; }    

        //Blogs section
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogImage> BlogsImages { get; set; }
        public DbSet<BlogTag> BlogsTags { get; set; }

        //Products section
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        
    }
}
