namespace ProniaWebApp.Services
{
    public class SettingService
    {
        private readonly AppDbContext _db;

        public SettingService(AppDbContext context)
        {
            _db = context;
        }

        //public async Task<Dictionary<string, string>> GetSettings()
        //{
        //    //return await _db.Settings.ToDictionary(s => s.Key, s => s.Value);
        //}
    }
}
