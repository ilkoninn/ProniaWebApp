namespace ProniaWebApp.Areas.Manage.ViewModels.EntityVM
{
    public class BaseAuditableEntityAdminVM : BaseEntityAdminVM
    {
        public DateTime CreatedDate {  get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
