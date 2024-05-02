using Core.Objects;
using Core.Objects.MyNwkUnitOfWork;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
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
            loggingBuilder.AddConsole()
                .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
            loggingBuilder.AddDebug();
        });
        
        services.AddDbContext<StartupDbContext>(x =>
        {
            x.UseSnakeCaseNamingConvention();
            x.EnableSensitiveDataLogging();
            x.UseNpgsql(connection);
        });
        
        // services.AddDbContext<UserContext>(options => options.UseNpgsql(connection));
        // services.AddDbContext<MyNwkDbContextBase<UserContext>>();
        
        AddServices(services, connection);
        
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            
        }).AddEntityFrameworkStores<StartupDbContext>().AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "myNwkAuth";
            options.LoginPath = "/account/login";
            options.AccessDeniedPath = "/account/accessdenied";
        });

        services.AddAuthorization(x =>
        {
            x.AddPolicy("UserArea", policy => { policy.RequireRole("user"); });
        });

        services.AddControllersWithViews(x =>
        {
            x.Conventions.Add(new UserAreaAuthorization("User", "UserArea"));
        }).AddSessionStateTempDataProvider();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
            });
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
            endpoints.MapAreaControllerRoute(
                name: "user",
                areaName: "User",
                pattern: "User/{controller=Profile}/{action=Index}/{id?}");
            endpoints.MapControllerRoute("user", "{controller=Account}/{action=Index}/{id?}");
            endpoints.MapControllerRoute("productAdd", "ProductAdd/{action=Index}/{id?}");
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
        });
    }

    private void AddServices(IServiceCollection services, string connection)
    {
        services.AddDbContextFactory<MyNwkDbContext>(t => t
            .UseNpgsql(connection)
            .UseLazyLoadingProxies()
            .UseSnakeCaseNamingConvention());
        services.AddDbContext<MyNwkDbContext>(t => t
            .UseNpgsql(connection)
            .UseLazyLoadingProxies()
            .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWorkProvider, UnitOfWorkProvider>();
    }
}