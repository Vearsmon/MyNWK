using Core.Objects.Markets;
using Core.Objects.Products;
using Core.Objects.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Objects.Orders;

public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.Property(t => t.OrderId).IsRequired();
        builder.Property(t => t.BuyerId).IsRequired();
        builder.Property(t => t.SellerId).IsRequired();
        builder.Property(t => t.MarketId).IsRequired();
        builder.Property(t => t.ProductId).IsRequired();
        builder.Property(t => t.CanceledBySeller).IsRequired();
        builder.Property(t => t.CanceledBySeller).IsRequired();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.BuyerId);

        builder
            .HasOne<Market>()
            .WithMany()
            .HasForeignKey(t => t.MarketId);

        builder
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(t => new { t.MarketId, t.ProductId });
    }
}