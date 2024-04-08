using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Repositories.Markets;

public class MarketInfoEntityConfiguration : IEntityTypeConfiguration<MarketInfoEntity>
{
    public void Configure(EntityTypeBuilder<MarketInfoEntity> builder)
    {
        builder.ToTable("market_infos");
        builder.HasKey(t => t.MarketId);

        builder
            .Property(t => t.Description)
            .HasColumnType("text");

        builder
            .Property(t => t.AutoHide)
            .HasDefaultValue(true)
            .IsRequired();
    }
}