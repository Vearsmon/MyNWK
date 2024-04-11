namespace Core.Repositories.Categories;

public class CategoriesRepository : ICategoriesRepository
{
    private readonly CategoryContext categoryContext;

    public CategoriesRepository(CategoryContext categoryContext)
    {
        this.categoryContext = categoryContext;
    }
}