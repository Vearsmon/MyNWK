namespace Core.Services.Orders;

public record OrderToCreateDto
{
    public int BuyerId;
    public List<OrderItemToCrateDto> Items = null!;
}

public record OrderItemToCrateDto(int SellerId, int MarketId, int ProductId);