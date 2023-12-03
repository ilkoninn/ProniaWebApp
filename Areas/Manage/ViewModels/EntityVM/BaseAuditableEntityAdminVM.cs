namespace ProniaWebApp.Areas.Manage.ViewModels.EntityVM
{
    public class BaseAuditableEntityVM : BaseEntityVM
    {
        public DateTime? CreatedDate {  get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
