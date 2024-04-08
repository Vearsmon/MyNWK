using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Repositories.Markets;

public class MarketInfoEntityConfiguration : IEntityTypeConfiguration<MarketInfoEntity>
{
    public void Configure(EntityTypeBuilder<MarketInfoEntity> builder)
    {
        throw new NotImplementedException();
    }
}