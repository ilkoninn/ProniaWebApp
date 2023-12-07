
namespace ProniaWebApp.Models
{
    public class Settings : BaseAuditableEntity
    {
        public string Key { get; set; }
        public string? Value { get; set; }

        [NotMapped]
        public IFormFile? LogoFile { get; set; }
    }
}