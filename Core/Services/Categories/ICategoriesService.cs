using Core.Objects.Categories;

namespace Core.Services.Categories;

public interface ICategoriesService
{
    public Task<List<Category>> GetAllAsync(RequestContext requestContext);
}