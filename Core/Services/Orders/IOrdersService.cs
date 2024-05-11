namespace Core.Services.Orders;

public interface IOrdersService
{
    public Task<List<Guid>> CreateOrdersAsync(
        RequestContext requestContext,
        CartDto cart);
}