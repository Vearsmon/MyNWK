using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Categories;

[EntityTypeConfiguration(typeof(CategoryEntityConfiguration))]
public class CategoryEntity
{
    public int Id { get; set; }
    
    public string Title { get; set; }
}