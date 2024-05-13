using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Services.Products;

[EntityTypeConfiguration(typeof(ClickOnProductMetricEntityTypeConfiguration))]
public class ClickOnProductMetric
{
    public int Id { get; set; }
    public int MarketId { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ClickOnProductMetricEntityTypeConfiguration : IEntityTypeConfiguration<ClickOnProductMetric>
{
    public void Configure(EntityTypeBuilder<ClickOnProductMetric> builder)
    {
        builder.ToTable("click_on_product_metric");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();
    }
}