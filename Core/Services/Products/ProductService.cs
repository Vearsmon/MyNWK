using System.Security.Cryptography.X509Certificates;
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
            .Select(p => Convert(p.product, userIdsByMarketId[p.product.MarketId], p.imageRef, p.product.Remained))
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
            .Select(p => Convert(p.product, userId, p.imageRef, p.product.Remained))
            .ToList();
    }
    
    public async Task<ProductDto?> GetProductByFullId(RequestContext requestContext, ProductFullId productFullId)
    {
        await using var unitOfWork = unitOfWorkProvider.Get();
        var product = await unitOfWork.ProductRepository
            .GetAsync(p => p
                    .Where(x => x.MarketId == productFullId.MarketId && x.ProductId == productFullId.ProductId), 
                requestContext.CancellationToken)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (product is null)
        {
            return null;
        }
        
        var productWithImageRef = await GetImageRefByMarketAndProductId(new List<Product> { product })
            .ConfigureAwait(false);
        
        return Convert(product, productFullId.UserId, productWithImageRef.FirstOrDefault().imageRef, product.Remained);
    }

    public async Task<List<ProductDto>> GetOrderProductsAsync(RequestContext requestContext, Guid orderId)
    {
        var userId = requestContext.UserId 
                     ?? throw new ArgumentException("UserId should not be null. User might not be authenticated");
        await using var unitOfWork = unitOfWorkProvider.Get();
        
        var productIds = await unitOfWork.OrdersRepository.GetAsync(
                r => r
                    .Where(m => m.OrderId == orderId)
                    .Select(m => m.ProductId), 
                requestContext.CancellationToken)
            .ConfigureAwait(false);

        var countDict = productIds
            .GroupBy(p => p)
            .ToDictionary(x => x.Key, x => x.Count());

        var products = await unitOfWork.ProductRepository.GetAsync(
                r => r
                    .Where(m => productIds.Any(x => x == m.ProductId)),
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
            .Select(p => Convert(p.product, userIdsByMarketId[p.product.MarketId], p.imageRef, countDict[p.product.ProductId]))
            .ToList();
    }

    public async Task<ProductDto?> UpdateProductInfoAsync(RequestContext requestContext, Dictionary<string, string> parametersToUpdate)
    {
        var userId = requestContext.UserId 
                     ?? throw new ArgumentException("UserId should not be null. User might not be authenticated");
        await using var unitOfWork = unitOfWorkProvider.Get();
        var productToChange = await unitOfWork.ProductRepository
            .GetAsync(p => p
                    .Where(t => t.ProductId == int.Parse(parametersToUpdate["productId"])),
                requestContext.CancellationToken).FirstOrDefaultAsync().ConfigureAwait(false);
        
        if (productToChange == null)
            throw new NotImplementedException();

        productToChange.Description = parametersToUpdate["description"];
        productToChange.Price = int.Parse(parametersToUpdate["price"]);
        productToChange.Title = parametersToUpdate["title"];
        productToChange.Remained = int.Parse(parametersToUpdate["remained"]);
        
        await unitOfWork.CommitAsync(requestContext.CancellationToken).ConfigureAwait(false);
        return Convert(productToChange, userId, null, productToChange.Remained);
    }
    
    public async Task DeleteProductByIdAsync(RequestContext requestContext, int productId)
    {
        await using var unitOfWork = unitOfWorkProvider.Get();
        await unitOfWork.ProductRepository.DeleteAsync(p => p.Where(t => t.ProductId == productId),
            requestContext.CancellationToken).ConfigureAwait(false);
        // await unitOfWork.CommitAsync(requestContext.CancellationToken).ConfigureAwait(false);
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
    
    private ProductDto Convert(Product product, int userId, string? imageRef, int number) =>
        new()
        {
            CategoryId = product.CategoryId,
            FullId = new ProductFullId(userId, product.MarketId, product.ProductId),
            ImageRef = imageRef,
            Price = product.Price,
            Remained = number,
            Title = product.Title,
            Description = product.Description ?? ""
        };
    
    
}