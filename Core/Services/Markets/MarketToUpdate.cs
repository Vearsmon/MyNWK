using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class MarketToUpdate
{
    [Required]
    public int Id { get; set; }
    
    [StringLength(128)]
    [DataType(DataType.Text)]
    public string Name { get; set; }
    
    public bool Closed { get; set; }
    
    [StringLength(512)]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }
    
    [DataType(DataType.Time)]
    public TimeOnly? WorksFrom { get; set; }

    [DataType(DataType.Time)]
    public TimeOnly? WorksTo { get; set; }
    
    public bool AutoHide { get; set; }
}