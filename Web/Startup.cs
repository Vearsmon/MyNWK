using Core.BlobStorage;
using Core.Crypto;
using Core.Objects;
using Core.Objects.MyNwkUnitOfWork;
using Core.Services.Categories;
using Core.Services.Markets;
using Core.Services.Orders;
using Core.Services.Products;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Web.Filters;
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
        AddServices(services, connection);

        services
            .AddMvc(options =>
            {
                options.Filters.Add(typeof(InputValidationActionFilter));
            });
        
        services
            .AddLogging(ConfigureLogging)
            .AddAuthorization(x => 
            { 
                x.AddPolicy("UserPolicy", policy => { policy.RequireClaim("AllowUserActions"); });
            })
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();
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
        services.AddDbContext<CoreDbContext>(t => t
            .UseNpgsql(connection)
            .UseLazyLoadingProxies()
            .UseSnakeCaseNamingConvention());
        services.AddScoped<Func<CoreDbContext>>(c => () => c.GetService<CoreDbContext>()!);
        // services.AddDbContext<CoreDbContext>(t => t
        //     .UseNpgsql(connection)
        //     .UseLazyLoadingProxies()
        //     .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWorkProvider, UnitOfWorkProvider>();
        services.AddSingleton<IBlobStorageClient, YdBlobStorageClient>();
        services.AddScoped<ITgAuthService, TgAuthService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrdersService, OrderService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IMarketsService, MarketsService>();
    }
    
    private void ConfigureLogging(ILoggingBuilder loggingBuilder) =>
        loggingBuilder
            .AddConsole()
            .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information)
            .AddDebug();
}