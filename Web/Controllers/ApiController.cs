using System.Security.Claims;
using Core;
using Core.Objects.MyNwkUnitOfWork;
using Core.Services.Categories;
using Core.Services.Markets;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api")]
public class ApiController : Controller
{
    private readonly ICategoriesService categoriesService;
    private readonly IMarketsService marketsService;

    public ApiController(
        ICategoriesService categoriesService,
        IMarketsService marketsService)
    {
        this.categoriesService = categoriesService;
        this.marketsService = marketsService;
    }
    
    [HttpGet]
    [Route("isAuthenticated")]
    public async Task<IActionResult> IsAuthenticated()
    {
        var identity = HttpContext.User.Identity;
        if (identity is null)
        {
            return Ok("non");
        }

        if (!identity.IsAuthenticated)
        {
            return Ok("non");
        }
        
        var expiresAtClaim = HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Expiration);
        var expiresAt = long.Parse(expiresAtClaim?.Value ?? "0");
        return Ok(DateTime.UtcNow.Ticks < expiresAt ? identity.Name : "non");
    }

    [HttpGet]
    [Route("get/all/categories")]
    public async Task<JsonResult> GetAllCategoriesAsync(CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        return Json(await categoriesService.GetAllAsync(requestContext));
    }

    [HttpGet]
    [Route("get/all/markets")]
    public async Task<JsonResult> GetAllMarketsAsync(CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        return Json(await marketsService.GetAllMarkets(requestContext));
    }
}