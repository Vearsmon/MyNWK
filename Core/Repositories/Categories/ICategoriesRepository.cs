namespace Core.Repositories.Categories;

public interface ICategoriesRepository
{
    IQueryable<CategoryEntity> getCategoryEntities();
    CategoryEntity GetCategoryEntityById(int id);
    void SaveCategoryEntity(CategoryEntity entity);
    void DeleteCategoryEntityById(int id);
}