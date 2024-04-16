namespace Domain.Objects;

public class MarketInfo
{
    public string? Description { get; set; }
    
    public TimeOnly? WorksFrom { get; set; }
    
    public TimeOnly? WorksTo { get; set; }
    
    public bool AutoHide { get; set; }
    
    public MarketInfo(
        string? description,
        TimeOnly? worksFrom,
        TimeOnly? worksTo,
        bool autoHide)
    {
        Description = description;
        WorksFrom = worksFrom;
        WorksTo = worksTo;
        AutoHide = autoHide;
    }
}