using Core.Objects.Categories;
using Core.Objects.Markets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Objects.Products;

public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
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
            .Property(t => t.Description)
            .HasColumnType("text");
        builder
            .Property(t => t.ImageLocation)
            .HasColumnType("varchar(255)");
        
        builder.Property(t => t.Remained).IsRequired();
        builder.Property(t => t.Reserved).IsRequired();
        builder.Property(t => t.Price).IsRequired();
        builder.Property(t => t.CreatedAt).HasColumnType("timestamptz").IsRequired();

        builder
            .HasOne<Market>()
            .WithMany(t => t.Products)
            .HasForeignKey(t => t.MarketId);

        builder
            .HasOne<Category>(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId);
    }
}