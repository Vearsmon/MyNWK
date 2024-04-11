using Core.Repositories.Markets;

namespace Core.Repositories.Products;

public class ProductEntity
{
    public int MarketId { get; set; }
    public MarketEntity Market { get; set; }
    
    public int ProductId { get; set; }
    
    public int? CategoryId { get; set; }
    // public CategoryEntity Category { get; set; }
    
    public string Title { get; set; }
    
    public string? ImageLocation { get; set; }
    
    public double Price { get; set; }
    
    public int Remained { get; set; }
    
}