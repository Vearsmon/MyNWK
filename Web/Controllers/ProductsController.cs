using Core;
using Core.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("products")]
public class ProductsController : Controller
{
    private readonly IProductService productService;

    public ProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet]
    [Route("get/all")]
    public async Task<JsonResult> GetAllAsync(
        int pageNumber,
        int batchSize,
        CancellationToken cancellationToken)
    {
        var requestContext = new RequestContext { CancellationToken = cancellationToken };
        return Json(await productService.GetAllProductsAsync(requestContext, pageNumber, batchSize));
    }
}