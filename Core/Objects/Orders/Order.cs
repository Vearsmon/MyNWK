using Microsoft.EntityFrameworkCore;

namespace Core.Objects.Orders;

[EntityTypeConfiguration(typeof(OrderEntityConfiguration))]
public class Order
{
    public Guid OrderId { get; init; }
    public int BuyerId { get; init; }
    public int MarketId { get; init; }
    public int ProductId { get; init; }
    public bool ReceivedByBuyer { get; init; }
    public bool CanceledBySeller { get; init; }
}