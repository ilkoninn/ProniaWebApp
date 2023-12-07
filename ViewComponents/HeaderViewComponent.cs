namespace ProniaWebApp.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;

        public HeaderViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting = await _db.Settings.ToDictionaryAsync(x => x.Key, x => x.Value);
            return View(setting);
        }
    }
}
