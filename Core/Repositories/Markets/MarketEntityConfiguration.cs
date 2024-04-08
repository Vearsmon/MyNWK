using Core.Repositories.Sellers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Repositories.Markets;

public class MarketEntityConfiguration : IEntityTypeConfiguration<MarketEntity>
{
    public void Configure(EntityTypeBuilder<MarketEntity> builder)
    {
        builder.ToTable("markets");
        builder.HasKey(t => t.Id);
        
        builder
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(t => t.Name)
            .HasColumnType("varchar(128)")
            .IsRequired();

        builder
            .Property(t => t.OwnerId)
            .IsRequired();

        builder
            .HasOne<SellerEntity>(t => t.Seller)
            .WithMany(t => t.Markets)
            .HasForeignKey(t => t.OwnerId);
    }
}