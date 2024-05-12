using System.ComponentModel.DataAnnotations;
using Web.Validation;

namespace Web.Models.ViewComponents;

public class ProductAddModel
{
    [Required(ErrorMessage = "Введите название товара")]
    [DataType(DataType.Text)]
    [StringLength(128)]
    public string Title { get; set; }

    [Required(ErrorMessage = "Введите цену товара")]
    [Range(0, int.MaxValue)]
    public double Price { get; set; }

    [Required(ErrorMessage = "Укажите количество оставшегося товара")]
    [Range(0, int.MaxValue)]
    public int Count { get; set; }

    [MaxFileSize(1_048_576)]
    [AllowedExtensions(new [] { ".jpg", ".jpeg", ".png", ".gif" })]
    public IFormFile? Image { get; set; }
    
    [StringLength(1024)]
    public string? Description { get; set; }
}