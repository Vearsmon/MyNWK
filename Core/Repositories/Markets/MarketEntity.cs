using Core.Repositories.Sellers;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Markets;

[EntityTypeConfiguration(typeof(MarketEntityConfiguration))]
public class MarketEntity
{
    public int Id { get; set; }
    
    public int OwnerId { get; set; }
    public SellerEntity Seller { get; set; }
    
    public string Name { get; set; }
    
    public bool Closed { get; set; }

    public MarketInfoEntity MarketInfo { get; set; }
}