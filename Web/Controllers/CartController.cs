using System.Text;
using Core;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Products;
using Core.Services.Orders;
using Core.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers;

[Route("cart")]
public class CartController : Controller
{
    private readonly IProductService productService;
    
    public CartController(ProductService productService)
    {
        this.productService = productService;
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<JsonResult> AddToCartWithCookie(
        CancellationToken cancellationToken,
        int productId,
        int marketId)
    {
        var orderRecords = await Deserialize();
        orderRecords.Add(new OrderDto 
        { 
            ProductId = productId, 
            MarketId = marketId
        });

        return await Serialize(orderRecords);
    }
    
    [HttpPost]
    [Route("remove")]
    public async Task<JsonResult> RemoveFromCartWithCookie(
        CancellationToken cancellationToken,
        int productId,
        int marketId,
        int sellerId)
    {
        var orderRecords = await Deserialize();
        var productToRemove = orderRecords
            .FirstOrDefault(p => p.ProductId == productId && p.MarketId == marketId);
        if (productToRemove != null)
            orderRecords.Remove(productToRemove);

        return await Serialize(orderRecords);
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<JsonResult> GetCartByCookie(CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        var userId = requestContext.UserId 
                     ?? throw new ArgumentException("UserId should not be null. User might not be authenticated");
        var orderRecords = await Deserialize();
        var result = new List<ProductDto>();
        foreach (var orderRecord in orderRecords)
        {
            result.Add(await productService.GetProductByFullId(
                requestContext, 
                new ProductFullId(userId, orderRecord.MarketId, orderRecord.ProductId)));
        }
        
        return Json(result);
    }

    private async Task<JsonResult> Serialize(List<OrderDto> orderRecords)
    {
        var data = JsonConvert.SerializeObject(orderRecords);
        var newBase64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        HttpContext.Response.Cookies.Append("products", newBase64Data);
        return Json(orderRecords);
    }

    private async Task<List<OrderDto>> Deserialize()
    {
        if (HttpContext.Request.Cookies.TryGetValue("products", out var base64Data))
            return ParseStringIntoOrder(base64Data);
        return [];
    }
    
    public List<OrderDto> ParseStringIntoOrder(string orderString)
    {
        var data = Encoding.UTF8.GetString(Convert.FromBase64String(orderString));
        var orderRecords = JsonConvert.DeserializeObject<List<OrderDto>>(data);
        return orderRecords ?? [];
    }
}