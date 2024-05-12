using Core.Services.Products;

namespace Core.Services.Orders;

public class FullOrder
{
    public Guid OrderId;
    public List<ProductDto> Products = null!;
}