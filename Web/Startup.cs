using Core.Repositories.Users;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace Web;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    [UsedImplicitly]
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var connection = Configuration.GetConnectionString("DefaultConnection")!;
        services.AddDbContextFactory<UserContext>(t => t
            .UseNpgsql(connection)
            .UseLazyLoadingProxies()
            .UseSnakeCaseNamingConvention());
        services.AddScoped<IUsersRepository, UsersRepository>();
        // services.AddDbContext<UserContext>(options => options.UseNpgsql(connection));

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
            });
        
        services.AddControllersWithViews()
            .AddSessionStateTempDataProvider();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseStaticFiles();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
        });
    }
}