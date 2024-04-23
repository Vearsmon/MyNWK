using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Objects.Markets;

public class MarketInfoEntityConfiguration : IEntityTypeConfiguration<MarketInfo>
{
    public void Configure(EntityTypeBuilder<MarketInfo> builder)
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