using Core.BlobStorage;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Products;

namespace Core.Services.Products;

public class ProductService : IProductService
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;
    private readonly IBlobStorageClient blobStorageClient;

    public ProductService(
        IUnitOfWorkProvider unitOfWorkProvider,
        IBlobStorageClient blobStorageClient)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.blobStorageClient = blobStorageClient;
    }
    
    public async Task<ProductFullId> CreateAsync(
        RequestContext requestContext,
        ProductToCreateDto productToCreate)
    {
        await using var unitOfWork = unitOfWorkProvider.Get();
        var market = await unitOfWork.MarketsRepository
            .GetAsync(
                r => r.Where(s => s.OwnerId == requestContext.UserId), 
                requestContext.CancellationToken)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        
        if (market is null)
        {
            throw new InvalidOperationException($"Could not find market with ownerId: {requestContext.UserId}");
        }
        
        return await market
            .AddProduct(unitOfWork, productToCreate, requestContext.CancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<string?> SaveImageAsync(RequestContext requestContext, byte[] image)
    {
        var key = await blobStorageClient
            .PutAsync(BlobContainers.ProductImages, image, requestContext.CancellationToken)
            .ConfigureAwait(false);
        return key.ToString();
    }

    public async Task<List<ProductDto>> GetAllProductsAsync(
        RequestContext requestContext,
        int batchNum = 0,
        int batchSize = 20,
        int? categoryId = null,
        int? marketId = null)
    {
        await using var unitOfWork = unitOfWorkProvider.Get();

        var products = await unitOfWork.ProductRepository.GetPageAsync(
                r => r.ProductOrderer(),
                p => p
                    .Where(t => marketId == null || t.MarketId == marketId)
                    .Where(t => categoryId == null || t.CategoryId == categoryId),
                batchNum,
                batchSize,
                requestContext.CancellationToken)
            .ConfigureAwait(false);

        var marketIds = products.Select(t => t.MarketId).ToArray();
        var userIdsByMarketId = await unitOfWork.MarketsRepository
            .GetAsync(
                r => r
                    .Where(t => marketIds.Any(id => id == t.Id)),
                requestContext.CancellationToken)
            .ToDictionaryAsync(t => t.Id, t => t.OwnerId)
            .ConfigureAwait(false);
        
        var productWithImageRef = await GetImageRefByMarketAndProductId(products)
            .ConfigureAwait(false);
        return productWithImageRef
            .Where(p => userIdsByMarketId.ContainsKey(p.product.MarketId))
            .Select(p => Convert(p.product, userIdsByMarketId[p.product.MarketId], p.imageRef))
            .ToList();
    }

    public async Task<List<ProductDto>> GetUserProductsAsync(RequestContext requestContext)
    {
        var userId = requestContext.UserId 
                     ?? throw new ArgumentException("UserId should not be null. User might not be authenticated");
        await using var unitOfWork = unitOfWorkProvider.Get();
        
        var products = await unitOfWork.MarketsRepository.GetAsync(
                r => r
                    .Where(m => m.OwnerId == userId)
                    .SelectMany(m => m.Products), 
                requestContext.CancellationToken)
            .ConfigureAwait(false);

        var productWithImageRef = await GetImageRefByMarketAndProductId(products)
            .ConfigureAwait(false);
        return productWithImageRef
            .Select(p => Convert(p.product, userId, p.imageRef))
            .ToList();
    }
    
    public async Task<ProductDto> GetProductByFullId(RequestContext requestContext, ProductFullId productFullId)
    {
        var unitOfWork = unitOfWorkProvider.Get();
        var userId = requestContext.UserId 
                     ?? throw new ArgumentException("UserId should not be null. User might not be authenticated");
        var product = await unitOfWork.ProductRepository
            .GetAsync(p => p
                    .Where(x => x.ProductId == productFullId.ProductId),
                requestContext.CancellationToken).FirstOrDefaultAsync().ConfigureAwait(false);
        if (product != null)
            return Convert(product, userId, null);
        return null;
    }

    private async Task<List<(Product product, string? imageRef)>> GetImageRefByMarketAndProductId(
        List<Product> products)
    {
        var result = new List<(Product product, string? imageRef)>();
        foreach (var product in products)
        {
            if (product.ImageLocation is null)
            {
                result.Add((product, null));
                continue;
            }

            var imageRef = await blobStorageClient
                .GetDownloadingUrlAsync(BlobContainers.ProductImages, Guid.Parse(product.ImageLocation))
                .ConfigureAwait(false);
            result.Add((product, imageRef));
        }

        return result;
    }
    
    private ProductDto Convert(Product product, int userId, string? imageRef) =>
        new()
        {
            CategoryId = product.CategoryId,
            FullId = new ProductFullId(userId, product.MarketId, product.ProductId),
            ImageRef = imageRef,
            Price = product.Price,
            Remained = product.Remained,
            Title = product.Title
        };
    
    
}