using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Products;
using Microsoft.EntityFrameworkCore;

namespace Core.Objects.Orders;

[EntityTypeConfiguration(typeof(OrderEntityConfiguration))]
public class Order
{
    public long Id { get; }
    public Guid OrderId { get; }
    public int BuyerId { get; }
    public int SellerId { get; }
    public int MarketId { get; }
    public int ProductId { get; }
    public OrderWorkflowState WorkflowState { get; private set; }
    public DateTime CreatedAt { get; }

    public Order()
    {

    }

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
        WorkflowState = OrderWorkflowState.Created;
    }

    public static async Task<Order> CreateAsync(
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
        var order = new Order(orderId, buyerId, sellerId, marketId, productId, createdAt);
        unitOfWork.OrdersRepository.Create(order);
        return order;
    }
    
    public async Task ConfirmAsync(RequestContext requestContext, IUnitOfWork unitOfWork)
    {
        var userId = requestContext.UserId;
        if (userId == SellerId)
        {
            await ConfirmBySellerAsync(requestContext, unitOfWork).ConfigureAwait(false);
        }
        else if (userId == BuyerId)
        {
            await ConfirmByBuyerAsync(requestContext, unitOfWork).ConfigureAwait(false);
        }
        else
        {
            throw new InvalidOperationException(
                $"Action not allowed for user: {requestContext.UserId}");
        }
    }
    
    public async Task CancelAsync(RequestContext requestContext, IUnitOfWork unitOfWork)
    {
        if (requestContext.UserId != SellerId)
        {
            throw new InvalidOperationException($"Action not allowed for user: {requestContext.UserId}");
        }

        if (WorkflowState is OrderWorkflowState.ConfirmedByBuyer or OrderWorkflowState.Cancelled)
        {
            return;
        }
        
        var product = await GetOrderProduct(requestContext, unitOfWork).ConfigureAwait(false);

        if (WorkflowState == OrderWorkflowState.ConfirmedBySeller)
        {
            product.Reserved = Math.Max(0, product.Reserved - 1);
        }
        WorkflowState = OrderWorkflowState.Cancelled;
    }

    private async Task ConfirmBySellerAsync(RequestContext requestContext, IUnitOfWork unitOfWork)
    {
        if (WorkflowState != OrderWorkflowState.Created)
        {
            return;
        }
        
        var product = await GetOrderProduct(requestContext, unitOfWork).ConfigureAwait(false);

        product.Reserved += 1;
        WorkflowState = OrderWorkflowState.ConfirmedBySeller;
    }

    private async Task ConfirmByBuyerAsync(RequestContext requestContext, IUnitOfWork unitOfWork)
    {
        if (WorkflowState != OrderWorkflowState.ConfirmedBySeller)
        {
            return;
        }

        var product = await GetOrderProduct(requestContext, unitOfWork).ConfigureAwait(false);

        product.Remained = Math.Max(0, product.Remained - 1);
        product.Reserved = Math.Max(0, product.Reserved - 1);
        WorkflowState = OrderWorkflowState.ConfirmedByBuyer;
    }

    private Task<Product> GetOrderProduct(RequestContext requestContext, IUnitOfWork unitOfWork) =>
        unitOfWork.ProductRepository
            .GetProduct(
                MarketId, 
                ProductId,
                requestContext.CancellationToken)!;
}