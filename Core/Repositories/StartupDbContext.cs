using Core.Crypto;
using Core.Repositories.Categories;
using Core.Repositories.Markets;
using Core.Repositories.Products;
using Core.Repositories.Rooms;
using Core.Repositories.Sellers;
using Core.Repositories.Users;
using Domain.Objects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories;

public class StartupDbContext : IdentityDbContext<IdentityUser>
{
    public StartupDbContext(DbContextOptions<StartupDbContext> options) : base(options) { }

    public DbSet<UserEntity> UsersEntities { get; set; }
    public DbSet<ProductEntity> ProductsEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserEntity>().HasData(new UserEntity()
        {
            Id = 1,
            TelegramUsername = "TestUser",
            TelegramId = 123,
        });
        
        modelBuilder.Entity<UserEntity>().HasKey("Id");

        modelBuilder.Entity<RoomEntity>().HasData(new RoomEntity()
        {
            Id = 1,
            Number = 500,
            Room = 4,
            Section = 'A'
        });
        
        modelBuilder.Entity<RoomEntity>().HasKey("Id");

        modelBuilder.Entity<CategoryEntity>().HasData(new CategoryEntity()
        {
            Id = 1,
            Title = "TestCategory"
        });
        
        modelBuilder.Entity<ProductEntity>().HasKey("ProductId");
        
        modelBuilder.Entity<MarketInfoEntity>().HasData(new MarketInfoEntity()
        {
            MarketId = 1,
            AutoHide = false
        });
        
        modelBuilder.Entity<MarketInfoEntity>().HasKey("MarketId");
        
        modelBuilder.Entity<SellerEntity>().HasData(new SellerEntity()
        {
            SellerId = 1,
            Markets = new List<MarketEntity>(),
            RoomId = 1,
            ShowRoom = true,
            UserId = 1
        });
        
        modelBuilder.Entity<SellerEntity>().HasKey("SellerId");

        modelBuilder.Entity<MarketEntity>(b =>
        {
            b.HasData(new MarketEntity(){
                Id = 1,
                Closed = true,
                Name = "TestMarket",
                OwnerId = 1,
            });
        });
        
        modelBuilder.Entity<MarketEntity>().HasKey("Id");
        
        modelBuilder.Entity<ProductEntity>().HasData(new ProductEntity()
            {
                Title = "TestProduct",
                ProductId = 1,
                Price = 100,
                Remained = 10,
                MarketId = 1
            }
        );
        
        modelBuilder.Entity<ProductEntity>().HasKey("ProductId");
    }
}