
namespace ProniaWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });


            var app = builder.Build();
            app.MapControllerRoute(
                name: "Home",
                pattern: "{controller=Home}/{action=Index}/{Id?}"
                );
            app.UseStaticFiles();

            app.Run();
        }
    }
}