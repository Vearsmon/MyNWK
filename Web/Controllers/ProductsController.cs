using Core;
using Core.Helpers;
using Core.Services.Orders;
using Core.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Policy = "UserPolicy")]
[Route("products")]
public class ProductsController : Controller
{
    private readonly IProductService productService;
    private readonly IOrdersService ordersService;

    public ProductsController(IProductService productService, IOrdersService ordersService)
    {
        this.productService = productService;
        this.ordersService = ordersService;
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("get/all")]
    public async Task<JsonResult> GetAllAsync(
        int pageNumber,
        int batchSize,
        int? categoryId,
        int? marketId,
        CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        return Json(await productService
            .GetAllProductsAsync(
                requestContext,
                pageNumber,
                batchSize,
                categoryId,
                marketId));
    }
    
    [HttpGet]
    [Route("get/byUser")]
    public async Task<JsonResult> GetByUserAsync(CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        return Json(await productService.GetUserProductsAsync(requestContext));
    }

    [HttpGet]
    [Route("get/byBuyer")]
    public async Task<JsonResult> GetByBuyerAsync(CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        var orderIds = await ordersService.GetBuyerOrderIdsAsync(requestContext);
        var result = new List<FullOrder>();
        foreach (var o in orderIds)
        {
            var products = await productService.GetOrderProductsAsync(requestContext, o);
            result.Add(new FullOrder() { OrderId = o, Products = products });
        }
        return Json(result);
    }

    [HttpGet]
    [Route("get/bySeller")]
    public async Task<JsonResult> GetBySellerAsync(CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        var orderIds = await ordersService.GetSellerOrderIdsAsync(requestContext);
        var result = new List<FullOrder>();
        foreach (var o in orderIds)
        {
            var products = await productService.GetOrderProductsAsync(requestContext, o);
            result.Add(new FullOrder() { OrderId = o, Products = products });
        }
        return Json(result);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateAsync(
        string title,
        int count,
        double price,
        CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        
        await using var productImageStream = HttpContext.Request.Form.Files.FirstOrDefault()?.OpenReadStream();
        string? imageLocation = null;
        if (productImageStream is not null)
        {
            var productImage = await productImageStream.ReadToEndAsync(32768, requestContext.CancellationToken);
            imageLocation =  await productService.SaveImageAsync(requestContext, productImage);
        }
        var productToCreate = new ProductToCreateDto
        {
            Title = title,
            Count = count,
            Price = price,
            ImageLocation = imageLocation
        };
        await productService.CreateAsync(requestContext, productToCreate);

        return Redirect("/Profile");
    }
}