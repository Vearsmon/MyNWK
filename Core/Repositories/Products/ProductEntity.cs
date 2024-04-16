using Core.Repositories.Categories;
using Core.Repositories.Markets;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Products;

[EntityTypeConfiguration(typeof(ProductEntityConfiguration))]
public class ProductEntity
{
    public int MarketId { get; set; }
    public virtual MarketEntity Market { get; set; }
    
    public int ProductId { get; set; }
    
    public int? CategoryId { get; set; }
    public virtual CategoryEntity? Category { get; set; }
    
    public string Title { get; set; }
    
    public string? ImageLocation { get; set; }
    
    public double Price { get; set; }
    
    public int Remained { get; set; }
    
}