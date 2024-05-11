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
    
    public async Task<Guid> CreateOrderAsync(RequestContext requestContext, OrderToCreateDto orderToCreate)
    {
        if (orderToCreate.Items.IsNullOrEmpty())
        {
            throw new ArgumentException(
                "Could not create empty order. Order items is empty",
                nameof(orderToCreate));
        }

        await using var unitOfWork = unitOfWorkProvider.Get();
        var orderId = Guid.NewGuid();
        await orderToCreate.Items
            .Select(item => Order.Create(
                unitOfWork,
                orderId,
                orderToCreate.BuyerId,
                item.SellerId,
                item.MarketId,
                item.ProductId,
                requestContext.CancellationToken))
            .ForEachAsync(unitOfWork.OrdersRepository.Create);
        await unitOfWork.CommitAsync(requestContext.CancellationToken).ConfigureAwait(false);
        return orderId;
    }
}