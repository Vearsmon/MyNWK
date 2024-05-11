namespace Core.Services.Orders;

public interface IOrdersService
{
    public Task<List<Guid>> CreateOrdersAsync(
        RequestContext requestContext,
        CartDto cart);
    
    public Task ConfirmAsync(
        RequestContext requestContext,
        Guid orderId);

    public Task CancelAsync(
        RequestContext requestContext,
        Guid orderId);

    public Task<List<Guid>> GetBuyerOrderIdsAsync(
        RequestContext requestContext);

    public Task<List<Guid>> GetSellerOrderIdsAsync(
        RequestContext requestContext);  
}