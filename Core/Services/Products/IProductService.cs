namespace Core.Services.Products;

public interface IProductService
{
    public Task<ProductFullId> CreateAsync(
        RequestContext requestContext,
        ProductToCreateDto productToCreate);

    public Task<List<ProductDto>> GetAllProductsAsync(
        RequestContext requestContext,
        int batchNum = 0,
        int batchSize = 20);

    public Task<List<ProductDto>> GetUserProductsAsync(RequestContext requestContext);
}