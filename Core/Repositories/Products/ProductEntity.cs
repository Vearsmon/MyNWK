using System.ComponentModel.DataAnnotations;
using Core.Repositories.Categories;
using Core.Repositories.Markets;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Products;

[EntityTypeConfiguration(typeof(ProductEntityConfiguration))]
public class ProductEntity
{
    public int MarketId { get; set; }
    
    [Display(Name = "Имя продавца")]
    public virtual MarketEntity Market { get; set; }
    public int ProductId { get; set; }
    public int? CategoryId { get; set; }
    
    [Display(Name = "Категория продукта")]
    public virtual CategoryEntity? Category { get; set; }
    
    [Display(Name = "Название продукта")]
    public string Title { get; set; }
    
    [Display(Name = "Изображение продукта")]
    public string? ImageLocation { get; set; }
    
    [Display(Name = "Цена продукта")]
    public double Price { get; set; }
    
    [Display(Name = "Сколько осталось в наличии")]
    public int Remained { get; set; }
}