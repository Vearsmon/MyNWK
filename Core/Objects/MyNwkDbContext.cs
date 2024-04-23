using Core.Objects.Categories;
using Core.Objects.Markets;
using Core.Objects.Products;
using Core.Objects.Rooms;
using Core.Objects.Sellers;
using Core.Objects.Users;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Objects;

public class MyNwkDbContext : DbContext
{
    [UsedImplicitly]
    public DbSet<Category> Categories { get; set; } = null!;

    [UsedImplicitly]
    public DbSet<Market> Markets { get; set; } = null!;

    [UsedImplicitly]
    public DbSet<MarketInfo> MarketInfos { get; set; } = null!;
    
    [UsedImplicitly]
    public DbSet<Product> Products { get; set; } = null!;
    
    [UsedImplicitly]
    public DbSet<Room> Rooms { get; set; } = null!;
    
    [UsedImplicitly]
    public DbSet<Seller> Sellers { get; set; } = null!;
    
    [UsedImplicitly] 
    public DbSet<User> Users { get; set; } = null!;
    
    public MyNwkDbContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}