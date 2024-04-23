using Microsoft.EntityFrameworkCore;

namespace Core.Objects.Markets;

[EntityTypeConfiguration(typeof(MarketInfoEntityConfiguration))]
public class MarketInfo
{
    public int MarketId { get; set; }
    
    public string? Description { get; set; }
    
    public TimeOnly? WorksFrom { get; set; }
    
    public TimeOnly? WorksTo { get; set; }
    
    public bool AutoHide { get; set; }
}