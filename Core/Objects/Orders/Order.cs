using Core.Helpers;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Products;
using Microsoft.EntityFrameworkCore;

namespace Core.Objects.Orders;

[EntityTypeConfiguration(typeof(OrderEntityConfiguration))]
public class Order
{
    public Guid OrderId { get; }
    public int BuyerId { get; }
    public int SellerId { get; }
    public int MarketId { get; }
    public int ProductId { get; }
    public bool ReceivedByBuyer { get; private set; }
    public bool CanceledBySeller { get; private set; }
    public DateTime CreatedAt { get; }

    private Order(
        Guid orderId,
        int buyerId,
        int sellerId,
        int marketId,
        int productId,
        DateTime createdAt)
    {
        OrderId = orderId;
        BuyerId = buyerId;
        SellerId = sellerId;
        MarketId = marketId;
        ProductId = productId;
        CreatedAt = createdAt;
    }

    public static async Task<Order> Create(
        IUnitOfWork unitOfWork,
        Guid orderId,
        int buyerId,
        int sellerId,
        int marketId,
        int productId,
        DateTime createdAt,
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository
            .GetProduct(
                marketId, 
                productId,
                cancellationToken)
            .ConfigureAwait(false)!;
        if (product.Remained - product.Reserved <= 0)
        {
            throw new InvalidOperationException("Could not crate order cause not products remained");
        }
        product.Reserved += 1;
        return new Order(orderId, buyerId, sellerId, marketId, productId, createdAt);
    }

    public async Task ConfirmAsync(RequestContext requestContext, IUnitOfWork unitOfWork)
    {
        if (requestContext.UserId != BuyerId)
        {
            throw new InvalidOperationException($"Action not allowed for user: {requestContext.UserId}");
        }

        await UpdateMarketProductAsync(requestContext, unitOfWork, true).ConfigureAwait(false);
        ReceivedByBuyer = true;
    }

    public async Task CancelAsync(RequestContext requestContext, IUnitOfWork unitOfWork)
    {
        if (requestContext.UserId != SellerId)
        {
            throw new InvalidOperationException($"Action not allowed for user: {requestContext.UserId}");
        }

        await UpdateMarketProductAsync(requestContext, unitOfWork, false).ConfigureAwait(false);
        CanceledBySeller = true;
    }

    private async Task UpdateMarketProductAsync(
        RequestContext requestContext,
        IUnitOfWork unitOfWork,
        bool isReceivedByBuyer)
    {
        var product = await unitOfWork.ProductRepository
            .GetProduct(
                MarketId, 
                ProductId,
                requestContext.CancellationToken)
            .ConfigureAwait(false)!;
        
        if (isReceivedByBuyer)
        {
            product.Remained = Math.Max(0, product.Remained - 1);
        }
        product.Reserved = Math.Max(0, product.Reserved - 1);
    }

}