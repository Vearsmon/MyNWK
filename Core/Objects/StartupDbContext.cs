using Core.Helpers;
using Core.Objects.Categories;
using Core.Objects.Markets;
using Core.Objects.Products;
using Core.Objects.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Objects;

public class StartupDbContext : IdentityDbContext<IdentityUser>
{
    public StartupDbContext(DbContextOptions<StartupDbContext> options) : base(options) { }

    public DbSet<User> UsersEntities { get; set; }
    public DbSet<Product> ProductsEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "1",
            Name = "user",
            NormalizedName = "USER"
        });

        modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
        {
            Id = "111",
            UserName = "user",
            NormalizedUserName = "USER",
            Email = "my@email.com",
            NormalizedEmail = "MY@EMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "qwerty"),
            SecurityStamp = string.Empty
        });

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = "1",
            UserId = "111"
        });
        
        modelBuilder.Entity<User>().HasData(new User()
        {
            Id = 1,
            TelegramUsername = "TestUser",
            TelegramId = 123,
        });
        
        modelBuilder.Entity<User>().HasKey("Id");


        modelBuilder.Entity<Category>().HasData(new Category()
        {
            Id = 1,
            Title = "TestCategory"
        });
        
        modelBuilder.Entity<Product>().HasKey("ProductId");
        
        modelBuilder.Entity<MarketInfo>().HasData(new MarketInfo()
        {
            MarketId = 1,
            AutoHide = false
        });
        
        modelBuilder.Entity<MarketInfo>().HasKey("MarketId");
        
        modelBuilder.Entity<Market>(b =>
        {
            b.HasData(new Market(){
                Id = 1,
                Closed = true,
                Name = "TestMarket",
                OwnerId = 1,
            });
        });
        
        modelBuilder.Entity<Market>().HasKey("Id");
        
        modelBuilder.Entity<Product>().HasData(new Product()
            {
                Title = "TestProduct",
                ProductId = 1,
                Price = 100,
                Remained = 10,
                MarketId = 1,
                CreatedAt = PreciseTimestampGenerator.Generate()
            }
        );
        
        modelBuilder.Entity<Product>().HasKey("ProductId");
    }
}