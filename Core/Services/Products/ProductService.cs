﻿using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Products;

namespace Core.Services.Products;

public class ProductService : IProductService
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;

    public ProductService(IUnitOfWorkProvider unitOfWorkProvider)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
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

    public async Task<List<ProductDto>> GetAllProductsAsync(
        RequestContext requestContext, 
        int batchNum = 0,
        int batchSize = 20)
    {
        await using var unitOfWork = unitOfWorkProvider.Get();

        var products = await unitOfWork.ProductRepository.GetPageAsync(
                r => r.ProductOrderer(),
                p => p,
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
        
        return products
            .Where(p => userIdsByMarketId.ContainsKey(p.MarketId))
            .Select(p => Convert(p, userIdsByMarketId[p.MarketId]))
            .ToList();
    }

    public async Task<List<ProductDto>> GetUserProductsAsync(RequestContext requestContext)
    {
        var userId = requestContext.UserId;
        await using var unitOfWork = unitOfWorkProvider.Get();
        
        var products = await unitOfWork.MarketsRepository.GetAsync(
                r => r
                    .Where(m => m.OwnerId == userId)
                    .SelectMany(m => m.Products), 
                requestContext.CancellationToken)
            .ConfigureAwait(false);

        return products.Select(p => Convert(p, userId)).ToList();
    }

    private static ProductDto Convert(Product product, int userId) =>
        new()
        {
            CategoryId = product.CategoryId,
            FullId = new ProductFullId(userId, product.MarketId, product.ProductId),
            ImageLocation = product.ImageLocation,
            Price = product.Price,
            Remained = product.Remained,
            Title = product.Title
        };
}