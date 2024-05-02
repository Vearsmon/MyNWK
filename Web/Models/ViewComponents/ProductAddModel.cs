using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewComponents;

public class ProductAddModel
{
    [Required(ErrorMessage = "Введите название товара")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Введите цену товара")]
    public double Price { get; set; }

    [Required(ErrorMessage = "Укажите количество оставшегося товара")]
    public int Remained { get; set; }
}