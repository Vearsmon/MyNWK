using Core.Crypto;
using Core.Objects;
using Core.Objects.MyNwkUnitOfWork;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Web.Service;

namespace Web;

public class Startup
{
    [UsedImplicitly]
    public IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        Configuration.Bind("ConnectionStrings", new Config());
        
        var connection = Configuration.GetConnectionString("DefaultConnection")!;
        
        services.AddLogging(loggingBuilder => {
            loggingBuilder.AddConsole().AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
            loggingBuilder.AddDebug();
        });
        AddServices(services, connection);
        
        // services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<StartupDbContext>().AddDefaultTokenProviders();

        // services.ConfigureApplicationCookie(options =>
        // {
        //     options.Cookie.Name = "myNwkAuth";
        //     options.Cookie.HttpOnly = true;
        //     options.LoginPath = "/baraholka";
        //     options.AccessDeniedPath = "/account/accessdenied";
        //     options.SlidingExpiration = true;
        // });
        
        services.AddAuthorization(x =>
        {
            x.AddPolicy("UserPolicy", policy => { policy.RequireClaim("AllowUserActions"); });
        });
        
        services.AddControllersWithViews(x =>
        {
            x.Conventions.Add(new UserAreaAuthorization("User", "UserArea"));
        }).AddSessionStateTempDataProvider();
        
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseRouting();
        app.UseStaticFiles();
        app.UseCookiePolicy();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("baraholka", "Baraholka/{action=Index}/{id?}");
            endpoints.MapControllerRoute("user", "{controller=Account}/{action=Index}/{id?}");
            endpoints.MapControllerRoute("productAdd", "ProductAdd/{action=Index}/{id?}");
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute("profile", "{area:exists}/{controller=Profile}/{action=Index}/{id?}");
        });
    }

    private void AddServices(IServiceCollection services, string connection)
    {
        services.AddDbContextFactory<CoreDbContext>(t => t
            .UseNpgsql(connection)
            .UseLazyLoadingProxies()
            .UseSnakeCaseNamingConvention());
        services.AddDbContext<CoreDbContext>(t => t
            .UseNpgsql(connection)
            .UseLazyLoadingProxies()
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWorkProvider, UnitOfWorkProvider>();
        services.AddScoped<ITgAuthService, TgAuthService>();
    }
}