using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Products;

public class ProductContext : MyNwkDbContextBase<ProductContext>
{
    [UsedImplicitly]
    public DbSet<ProductEntity> Products { get; } = null!;

    public ProductContext(DbContextOptions<ProductContext> options) 
        : base(options)
    {
    }
}