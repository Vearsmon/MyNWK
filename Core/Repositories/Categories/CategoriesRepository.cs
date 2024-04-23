using Microsoft.EntityFrameworkCore;

namespace Core.Repositories.Categories;

public class CategoriesRepository : ICategoriesRepository
{
    private readonly CategoryContext categoryContext;

    public CategoriesRepository(CategoryContext categoryContext)
    {
        this.categoryContext = categoryContext;
    }

    public IQueryable<CategoryEntity> getCategoryEntities()
    {
        return categoryContext.Categories;
    }

    public CategoryEntity GetCategoryEntityById(int id)
    {
        return categoryContext.Categories.FirstOrDefault(c => c.Id == id);
    }

    public void SaveCategoryEntity(CategoryEntity entity)
    {
        if (entity.Id == default)
        {
            categoryContext.Entry(entity).State = EntityState.Added;
        }
        else
        {
            categoryContext.Entry(entity).State = EntityState.Modified;
        }

        categoryContext.SaveChanges();
    }

    public void DeleteCategoryEntityById(int id)
    {
        categoryContext.Categories.Remove(new CategoryEntity { Id = id });
        categoryContext.SaveChanges();
    }
}