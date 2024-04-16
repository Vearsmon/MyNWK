using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Markets;

public class MarketContext : MyNwkDbContextBase<MarketContext>
{
    [UsedImplicitly]
    public DbSet<MarketEntity> Markets { get; set; } = null!;

    [UsedImplicitly]
    public DbSet<MarketInfoEntity> MarketInfos { get; set; } = null!;

    public MarketContext(DbContextOptions<MarketContext> options) 
        : base(options)
    {
    }
}