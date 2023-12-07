namespace ProniaWebApp.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _db;

        public LayoutService(AppDbContext context)
        {
            _db = context;
        }

        public async Task<Dictionary<string, string>> GetSetting()
        {
            Dictionary<string, string> setting = _db.Settings.ToDictionary(s => s.Key, s => s.Value);
            return setting;
        }
    }
}
