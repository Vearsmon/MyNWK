using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Sellers;

public class SellerContext : MyNwkDbContextBase<SellerContext>
{
    [UsedImplicitly]
    public DbSet<SellerEntity> Sellers { get; set; } = null!;

    public SellerContext(DbContextOptions<SellerContext> options)
        : base(options)
    {
    }
}