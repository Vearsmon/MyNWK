using Core.Repositories.Categories;
using Core.Repositories.Markets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Repositories.Products;

public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(t => new
        {
            t.MarketId,
            t.ProductId
        });

        builder
            .Property(t => t.ProductId)
            .ValueGeneratedOnAdd();

        builder
            .Property(t => t.Title)
            .HasColumnType("varchar(128)")
            .IsRequired();
        builder
            .Property(t => t.ImageLocation)
            .HasColumnType("varchar(255)");
        
        builder.Property(t => t.Remained).IsRequired();
        builder.Property(t => t.Price).IsRequired();

        builder
            .HasOne<MarketEntity>(t => t.Market)
            .WithMany(t => t.Products)
            .HasForeignKey(t => t.MarketId);

        builder
            .HasOne<CategoryEntity>(t => t.Category)
            .WithOne()
            .HasForeignKey<ProductEntity>(t => t.CategoryId);
    }
}