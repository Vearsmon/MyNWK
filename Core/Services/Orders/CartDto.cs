namespace Core.Services.Orders;

public record CartDto
{
    public int BuyerId;
    public List<OrderItemToCreateDto> Items = null!;
}

public record OrderItemToCreateDto(int SellerId, int MarketId, int ProductId);