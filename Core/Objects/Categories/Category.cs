using Microsoft.EntityFrameworkCore;

namespace Core.Objects.Categories;

[EntityTypeConfiguration(typeof(CategoryEntityConfiguration))]
public class Category
{
    public int Id { get; set; }
    
    public string Title { get; set; }
}