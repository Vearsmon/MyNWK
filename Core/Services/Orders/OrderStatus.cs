using Core.Objects.Orders;
using Core.Services.Products;

namespace Core.Services.Orders;

public class OrderStatus
{
    public Guid OrderId { get; set; }
    public OrderWorkflowState WorkflowState { get; set; }
    public int BuyerId { get; set; }
    public int SellerId { get; set; }
}