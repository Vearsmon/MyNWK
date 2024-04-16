using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Markets;

[EntityTypeConfiguration(typeof(MarketInfoEntityConfiguration))]
public class MarketInfoEntity
{
    public int MarketId { get; set; }
    
    public string? Description { get; set; }
    
    public TimeOnly? WorksFrom { get; set; }
    
    public TimeOnly? WorksTo { get; set; }
    
    public bool AutoHide { get; set; }
}