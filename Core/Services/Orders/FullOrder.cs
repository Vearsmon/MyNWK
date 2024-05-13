using Core.Objects.Orders;
using Core.Services.Products;

namespace Core.Services.Orders;

public class FullOrder
{
    public Guid OrderId { get; set; }
    public List<ProductDto> Products { get; set; } = null!;
    public OrderWorkflowState WorkflowState { get; set; }
    public int BuyerId { get; set; }
    public int SellerId { get; set; }
}