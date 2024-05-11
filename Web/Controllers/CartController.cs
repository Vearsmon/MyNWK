using System.Text;
using Core;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Products;
using Core.Services.Orders;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers;

[Route("cart")]
public class CartController : Controller
{
    [HttpPost]
    [Route("add")]
    public async Task<JsonResult> AddToCartWithCookie(
        CancellationToken cancellationToken,
        string productId,
        string marketId)
    {
        var orderRecords = await Deserialize();
        orderRecords.Add(new OrderRecord 
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
        string productId,
        string marketId,
        string sellerId)
    {
        var orderRecords = await Deserialize();
        var productToRemove = orderRecords
            .FirstOrDefault(p => p.ProductId == productId && p.MarketId == marketId && );
        if (productToRemove != null)
            orderRecords.Remove(productToRemove);

        return await Serialize(orderRecords);
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<JsonResult> GetCartByCookie(CancellationToken cancellationToken)
    {
        var orderRecords = await Deserialize();
        
        // сервис -> продукты -> жсон
        
        return Json(orderRecords);
    }

    private async Task<JsonResult> Serialize(List<OrderRecord> orderRecords)
    {
        var data = JsonConvert.SerializeObject(orderRecords);
        var newBase64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        HttpContext.Response.Cookies.Append("products", newBase64Data);
        return Json(orderRecords);
    }

    private async Task<List<OrderRecord>> Deserialize()
    {
        if (HttpContext.Request.Cookies.TryGetValue("products", out var base64Data))
            return ParseStringIntoOrder(base64Data);
        return [];
    }
    
    public List<OrderRecord> ParseStringIntoOrder(string orderString)
    {
        var data = Encoding.UTF8.GetString(Convert.FromBase64String(orderString));
        var orderRecords = JsonConvert.DeserializeObject<List<OrderRecord>>(data);
        return orderRecords ?? [];
    }
}