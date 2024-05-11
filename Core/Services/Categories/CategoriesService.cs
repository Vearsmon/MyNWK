using Core.Objects.Categories;
using Core.Objects.MyNwkUnitOfWork;

namespace Core.Services.Categories;

public class CategoriesService : ICategoriesService
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;

    public CategoriesService(IUnitOfWorkProvider unitOfWorkProvider)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
    }

    public async Task<List<Category>> GetAllAsync(RequestContext requestContext)
    {
        await using var unitOfWork = unitOfWorkProvider.Get();
        var categories = await unitOfWork.CategoriesRepository.GetAsync(
                r => r
                    .OrderBy(c => c.Title)
                    .ThenBy(c => c.Id),
                requestContext.CancellationToken)
            .ConfigureAwait(false);

        return categories;
    }
}