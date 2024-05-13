using System.Text;
using Core;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Products;
using Core.Services.Orders;
using Core.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers;

[Route("cart")]
[Authorize(Policy = "UserPolicy")]
public class CartController : Controller
{
    private readonly IProductService productService;
    private readonly IOrdersService orderService;
    
    public CartController(
        IProductService productService, 
        IOrdersService orderService)
    {
        this.productService = productService;
        this.orderService = orderService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View("~/Pages/Cart.cshtml");
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<JsonResult> AddToCartWithCookie(
        CancellationToken cancellationToken)
    {
        var form = await HttpContext.Request.ReadFormAsync(cancellationToken);
        
        var orderRecords = Deserialize();
        for (var _ = 0; _ < int.Parse(form["count"].ToString()); _++)
            orderRecords.Add(new OrderDto 
            { 
                ProductId = int.Parse(form["productId"].ToString()), 
                MarketId = int.Parse(form["marketId"].ToString()),
                SellerId = int.Parse(form["sellerId"].ToString())
            });
        
        return Serialize(orderRecords);
    }
    
    [HttpPost]
    [Route("remove")]
    public async Task<JsonResult> RemoveFromCartWithCookie(
        CancellationToken cancellationToken)
    {
        var form = await HttpContext.Request.ReadFormAsync(cancellationToken);
        
        var orderRecords = Deserialize();
        var productToRemove = orderRecords
            .FirstOrDefault(p => 
                p.ProductId == int.Parse(form["productId"].ToString()) && 
                p.MarketId == int.Parse(form["marketId"].ToString()) && 
                p.SellerId == int.Parse(form["sellerId"].ToString()));
        if (productToRemove != null)
            orderRecords.Remove(productToRemove);

        return Serialize(orderRecords);
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<JsonResult> GetCartByCookie(CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        var userId = requestContext.UserId 
                     ?? throw new ArgumentException("UserId should not be null. User might not be authenticated");
        var orderRecords = Deserialize();
        var result = new List<ProductDto>();
        foreach (var orderRecord in orderRecords)
        {
            result.Add(await productService.GetProductByFullId(
                requestContext, 
                new ProductFullId(orderRecord.SellerId, orderRecord.MarketId, orderRecord.ProductId)));
        }
        
        return Json(result);
    }

    [HttpPost]
    [Route("accept")]
    public async Task<JsonResult> AcceptCartOrderByCookie(CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        var cartProducts = Deserialize();
        var cart = new CartDto
        {
            BuyerId = requestContext.UserId ?? -1,
            Items = cartProducts.Select(p => new OrderItemToCreateDto(p.SellerId, p.MarketId, p.ProductId)).ToList()
        };

        Serialize(new List<OrderDto>());
        var result = await orderService.CreateOrdersAsync(requestContext, cart);
        return Json(result);
    }

    private JsonResult Serialize(List<OrderDto> orderRecords)
    {
        var data = JsonConvert.SerializeObject(orderRecords);
        var newBase64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        HttpContext.Response.Cookies.Append("products", newBase64Data);
        return Json(orderRecords);
    }

    private List<OrderDto> Deserialize()
    {
        return HttpContext.Request.Cookies.TryGetValue("products", out var base64Data) 
            ? ParseStringIntoOrder(base64Data)
            : new List<OrderDto>();
    }
    
    private List<OrderDto> ParseStringIntoOrder(string orderString)
    {
        var data = Encoding.UTF8.GetString(Convert.FromBase64String(orderString));
        var orderRecords = JsonConvert.DeserializeObject<List<OrderDto>>(data);
        return orderRecords ?? new List<OrderDto>();
    }
}

public class OrderParams
{
    public int Count { get; set; }
    public int MarketId { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
}