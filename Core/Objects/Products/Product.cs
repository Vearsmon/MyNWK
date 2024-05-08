using Core.Objects.Categories;
using Core.Objects.Markets;
using Microsoft.EntityFrameworkCore;

namespace Core.Objects.Products;

[EntityTypeConfiguration(typeof(ProductEntityConfiguration))]
public class Product
{
    public int MarketId { get; set; }
    public int ProductId { get; set; }
    public int? CategoryId { get; set; }
    public string Title { get; set; }
    public string? ImageLocation { get; set; }
    public double Price { get; set; }
    public int Remained { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual Category? Category { get; set; }
}