using Core;
using Core.Helpers;
using Core.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Policy = "UserPolicy")]
[Route("products")]
public class ProductsController : Controller
{
    private readonly IProductService productService;

    public ProductsController(IProductService productService)
    {
        this.productService = productService;
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
    
    [HttpPost]
    [Route("create")]
    public async Task<JsonResult> CreateAsync(
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
        
        return Json(await productService.CreateAsync(requestContext, productToCreate));
    }
}