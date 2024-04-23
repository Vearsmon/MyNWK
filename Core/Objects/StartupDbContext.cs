using Core.Objects.Categories;
using Core.Objects.Markets;
using Core.Objects.Products;
using Core.Objects.Rooms;
using Core.Objects.Sellers;
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
        
        modelBuilder.Entity<User>().HasData(new User()
        {
            Id = 1,
            TelegramUsername = "TestUser",
            TelegramId = 123,
        });
        
        modelBuilder.Entity<User>().HasKey("Id");

        modelBuilder.Entity<Room>().HasData(new Room()
        {
            Id = 1,
            FrameNumber = 500,
            RoomNumber = 4,
            Section = 'A'
        });
        
        modelBuilder.Entity<Room>().HasKey("Id");

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
        
        modelBuilder.Entity<Seller>().HasData(new Seller()
        {
            Markets = new List<Market>(),
            RoomId = 1,
            ShowRoom = true,
            UserId = 1
        });
        
        modelBuilder.Entity<Seller>().HasKey("SellerId");

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
                MarketId = 1
            }
        );
        
        modelBuilder.Entity<Product>().HasKey("ProductId");
    }
}