namespace ProniaWebApp.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        AppDbContext _db;

        public FooterViewComponent(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting = await _db.Settings.ToDictionaryAsync(x => x.Key, x => x.Value);
            return View(setting);
        }
    }
}
