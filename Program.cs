
using Microsoft.AspNetCore.Identity;

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
            builder.Services.AddScoped<LayoutService>();
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // Password settings.
                options.Password.RequiredLength = 8;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_.";

            }).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            var app = builder.Build();

            app.MapControllerRoute(
                name: "AdminPanel",
                pattern: "{area:exists}/{controller=AdminHome}/{action=Index}/{Id?}"
                );
            app.MapControllerRoute(
                name: "Home",
                pattern: "{controller=Home}/{action=Index}/{Id?}"
                );

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
    }
}