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
}