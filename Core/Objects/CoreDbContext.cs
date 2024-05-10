using Core.Objects.Categories;
using Core.Objects.Markets;
using Core.Objects.Products;
using Core.Objects.Users;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Objects;

public class CoreDbContext : DbContext
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
    public DbSet<User> Users { get; set; } = null!;

    public CoreDbContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}