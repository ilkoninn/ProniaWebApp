
namespace ProniaWebApp.Areas.Manage.ViewModels
{
    public class SliderVM : BaseAuditableEntityAdminVM
    {
        public string SubTitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
