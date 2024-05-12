using Core.Services.Products;

namespace Core.Services.Orders;

public class OrderStatus
{
    public Guid OrderId { get; set; }
    public bool ReceivedByBuyer { get; set; }
    public bool CanceledBySeller { get; set; }
}