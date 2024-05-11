using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewComponents;

public class ProductAddModel
{
    [Required(ErrorMessage = "Введите название товара")]
    [StringLength(128)]
    public string Title { get; set; }

    [Required(ErrorMessage = "Введите цену товара")]
    [Range(0, int.MaxValue)]
    public double Price { get; set; }

    [Required(ErrorMessage = "Укажите количество оставшегося товара")]
    [Range(0, int.MaxValue)]
    public int Count { get; set; }
}