namespace ProniaWebApp.Areas.Manage.ViewModels
{
    public class ProductVM : BaseAuditableEntityVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string CategoryId { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public List<int>? TagIds { get; set; }
        public ICollection<Tag>? Tags { get; set; }

    }
}
