using System.ComponentModel.DataAnnotations;

namespace Core.Services.Markets;

public class MarketToUpdateDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public bool Closed { get; set; }
    
    public string? Description { get; set; }
    
    public TimeOnly? WorksFrom { get; set; }

    public TimeOnly? WorksTo { get; set; }
    
    public bool AutoHide { get; set; }
}