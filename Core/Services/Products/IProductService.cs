namespace Core.Services.Products;

public interface IProductService
{
    public Task<ProductFullId> CreateAsync(
        RequestContext requestContext,
        ProductToCreateDto productToCreate);

    public Task<string?> SaveImageAsync(
        RequestContext requestContext,
        byte[] image);
    
    public Task<List<ProductDto>> GetAllProductsAsync(
        RequestContext requestContext,
        int batchNum = 0,
        int batchSize = 20,
        int? categoryId = null,
        int? marketId = null);

    public Task<ProductDto> GetProductByFullId(
        RequestContext requestContext, 
        ProductFullId productFullId);

    public Task<List<ProductDto>> GetUserProductsAsync(RequestContext requestContext);
}