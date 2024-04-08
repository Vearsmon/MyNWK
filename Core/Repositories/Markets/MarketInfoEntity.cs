namespace Core.Repositories.Markets;

public class MarketInfoEntity
{
    public int MarketId { get; set; }
    
    public string? Description { get; set; }
    
    public TimeOnly? WorksFrom { get; set; }
    
    public TimeOnly? WorksTo { get; set; }
    
    public bool AutoHide { get; set; }
}