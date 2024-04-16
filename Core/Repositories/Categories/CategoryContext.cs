using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Categories;

public class CategoryContext : MyNwkDbContextBase<CategoryContext>
{
    [UsedImplicitly]
    public DbSet<CategoryEntity> Categories { get; set; } = null!;

    public CategoryContext(DbContextOptions<CategoryContext> options) 
        : base(options)
    {
    }
}