using Core.Objects.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Objects.Markets;

public class MarketEntityConfiguration : IEntityTypeConfiguration<Market>
{
    public void Configure(EntityTypeBuilder<Market> builder)
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
            .HasOne<User>(t => t.User)
            .WithOne(t => t.Market)
            .HasForeignKey<Market>(t => t.OwnerId);

        builder
            .HasOne<MarketInfo>(t => t.MarketInfo)
            .WithOne()
            .HasForeignKey<MarketInfo>(t => t.MarketId);
    }
}