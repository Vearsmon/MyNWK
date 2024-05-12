using Core.Helpers;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Orders;

namespace Core.Services.Orders;

public class OrderService : IOrdersService
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;

    public OrderService(IUnitOfWorkProvider unitOfWorkProvider)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
    }

    public async Task<List<Guid>> GetBuyerOrderIdsAsync(RequestContext requestContext)
    {
        var userId = requestContext.UserId 
                     ?? throw new ArgumentException("UserId should not be null. User might not be authenticated");
        await using var unitOfWork = unitOfWorkProvider.Get();
        
        var OrderIds = await unitOfWork.OrdersRepository.GetAsync(
                r => r
                    .Where(m => m.BuyerId == userId)
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => m.OrderId)
                    .Distinct(),
                requestContext.CancellationToken)
            .ConfigureAwait(false);
        return OrderIds;
    }

    public async Task<List<Guid>> GetSellerOrderIdsAsync(RequestContext requestContext)
    {
        var userId = requestContext.UserId 
                     ?? throw new ArgumentException("UserId should not be null. User might not be authenticated");
        await using var unitOfWork = unitOfWorkProvider.Get();
        
        var OrderIds = await unitOfWork.OrdersRepository.GetAsync(
                r => r
                    .Where(m => m.SellerId == userId)
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => m.OrderId)
                    .Distinct(), 
                requestContext.CancellationToken)
            .ConfigureAwait(false);
        return OrderIds;
    }
    
    public async Task<List<Guid>> CreateOrdersAsync(RequestContext requestContext, CartDto cart)
    {
        if (cart.Items.IsNullOrEmpty())
        {
            throw new ArgumentException(
                "Could not create empty order. Order items is empty",
                nameof(cart));
        }

        await using var unitOfWork = unitOfWorkProvider.Get();
        var ordersWithId = cart.Items
            .GroupBy(t => t.MarketId)
            .Select(g => (Group: g, OrderId: Guid.NewGuid()))
            .ToList();
        await ordersWithId
            .SelectMany(orderWithId => orderWithId.Group
                .Select(item =>
                    Order.Create(
                        unitOfWork,
                        orderWithId.OrderId,
                        cart.BuyerId,
                        item.SellerId,
                        item.MarketId,
                        item.ProductId,
                        requestContext.CancellationToken)))
            .ForEachAsync(unitOfWork.OrdersRepository.Create)
            .ConfigureAwait(false);
        await unitOfWork.CommitAsync(requestContext.CancellationToken).ConfigureAwait(false);
        return ordersWithId.Select(t => t.OrderId).ToList();
    }

    public async Task ConfirmAsync(
        RequestContext requestContext, 
        Guid orderId)
    {
        await using var unitOfWork = unitOfWorkProvider.Get();
        var orders = await unitOfWork.OrdersRepository
            .GetOrder(orderId, requestContext.CancellationToken)
            .ConfigureAwait(false);
        
        await orders
            .ForEachAsync(t => t.ConfirmAsync(requestContext, unitOfWork))
            .ConfigureAwait(false);
        await unitOfWork.CommitAsync(requestContext.CancellationToken).ConfigureAwait(false);
    }
    
    public async Task CancelAsync(
        RequestContext requestContext, 
        Guid orderId)
    {
        await using var unitOfWork = unitOfWorkProvider.Get();
        var orders = await unitOfWork.OrdersRepository
            .GetOrder(orderId, requestContext.CancellationToken)
            .ConfigureAwait(false);

        await orders
            .ForEachAsync(t => t.CancelAsync(requestContext, unitOfWork))
            .ConfigureAwait(false);
        await unitOfWork.CommitAsync(requestContext.CancellationToken).ConfigureAwait(false);
    }
}