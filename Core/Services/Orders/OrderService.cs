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

    public async Task<List<OrderStatus>> GetBuyerOrderIdsAsync(RequestContext requestContext)
    {
        var userId = requestContext.UserId 
                     ?? throw new ArgumentException("UserId should not be null. User might not be authenticated");
        await using var unitOfWork = unitOfWorkProvider.Get();
        
        var Orders = await unitOfWork.OrdersRepository.GetAsync(
                r => r
                    .Where(m => m.BuyerId == userId)
                    .Select(m => new { m.OrderId, m.CanceledBySeller, m.ReceivedByBuyer, m.CreatedAt })
                    .Distinct()
                    .OrderBy(m => m.ReceivedByBuyer || m.CanceledBySeller)
                    .ThenByDescending(m => m.CreatedAt), 
                requestContext.CancellationToken)
            .ConfigureAwait(false);
        return Orders
            .Select(o => new OrderStatus() {
            OrderId = o.OrderId, 
            CanceledBySeller = o.CanceledBySeller, 
            ReceivedByBuyer = o.ReceivedByBuyer
            })
            .ToList();
    }

    public async Task<List<OrderStatus>> GetSellerOrderIdsAsync(RequestContext requestContext)
    {
        var userId = requestContext.UserId 
                     ?? throw new ArgumentException("UserId should not be null. User might not be authenticated");
        await using var unitOfWork = unitOfWorkProvider.Get();
        
        var Orders = await unitOfWork.OrdersRepository.GetAsync(
                r => r
                    .Where(m => m.SellerId == userId)
                    .Select(m => new { m.OrderId, m.CanceledBySeller, m.ReceivedByBuyer, m.CreatedAt })
                    .Distinct()
                    .OrderBy(m => m.ReceivedByBuyer || m.CanceledBySeller)
                    .ThenByDescending(m => m.CreatedAt), 
                requestContext.CancellationToken)
            .ConfigureAwait(false);
        return Orders
            .Select(o => new OrderStatus() {
            OrderId = o.OrderId, 
            CanceledBySeller = o.CanceledBySeller, 
            ReceivedByBuyer = o.ReceivedByBuyer
            })
            .ToList();
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

        var orderCreatedIds = new List<Guid>();
        foreach (var (items, orderId) in ordersWithId)
        {
            var createdAt = PreciseTimestampGenerator.Generate();
            foreach (var item in items)
            {
                await Order.CreateAsync(
                        unitOfWork,
                        orderId,
                        cart.BuyerId,
                        item.SellerId,
                        item.MarketId,
                        item.ProductId,
                        createdAt,
                        requestContext.CancellationToken)
                    .ConfigureAwait(false);
            }
            orderCreatedIds.Add(orderId);
        }
        
        await unitOfWork.CommitAsync(requestContext.CancellationToken).ConfigureAwait(false);
        return orderCreatedIds;
    }

    public async Task ConfirmAsync(
        RequestContext requestContext, 
        Guid orderId)
    {
        await using var unitOfWork = unitOfWorkProvider.Get();
        var orders = await unitOfWork.OrdersRepository
            .GetOrder(orderId, requestContext.CancellationToken)
            .ConfigureAwait(false);
        
        foreach (var order in orders)
        {
            await order.ConfirmAsync(requestContext, unitOfWork).ConfigureAwait(false);
        }
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

        foreach (var order in orders)
        {
            await order.CancelAsync(requestContext, unitOfWork).ConfigureAwait(false);
        }
        await unitOfWork.CommitAsync(requestContext.CancellationToken).ConfigureAwait(false);
    }
}