namespace Core.Services.Orders;

public interface IOrdersService
{
    public Task<Guid> CreateOrderAsync(
        RequestContext requestContext,
        OrderToCreateDto orderToCreate);
}