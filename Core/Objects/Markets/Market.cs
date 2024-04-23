using Core.Objects.Products;
using Core.Objects.Sellers;
using Microsoft.EntityFrameworkCore;

namespace Core.Objects.Markets;

[EntityTypeConfiguration(typeof(MarketEntityConfiguration))]
public class Market
{
    public int Id { get; set; }
    
    public int OwnerId { get; set; }
    public virtual Seller Seller { get; set; }
    
    public string Name { get; set; }
    
    public bool Closed { get; set; }

    public virtual MarketInfo MarketInfo { get; set; }
    
    public virtual List<Product> Products { get; set; }
}