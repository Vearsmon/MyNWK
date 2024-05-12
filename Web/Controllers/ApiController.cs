using System.Security.Claims;
using Core;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Users;
using Core.Services.Categories;
using Core.Services.Markets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api")]
public class ApiController : Controller
{
    private readonly ICategoriesService categoriesService;
    private readonly IMarketsService marketsService;
    private readonly IUnitOfWorkProvider unitOfWorkProvider;

    public ApiController(
        ICategoriesService categoriesService,
        IMarketsService marketsService,
        IUnitOfWorkProvider unitOfWorkProvider)
    {
        this.categoriesService = categoriesService;
        this.marketsService = marketsService;
        this.unitOfWorkProvider = unitOfWorkProvider;
    }
    
    [HttpGet]
    [Route("isAuthenticated")]
    public Task<IActionResult> IsAuthenticated()
    {
        var identity = HttpContext.User.Identity;
        if (identity is null)
        {
            return Task.FromResult<IActionResult>(Ok("non"));
        }

        if (!identity.IsAuthenticated)
        {
            return Task.FromResult<IActionResult>(Ok("non"));
        }
        
        var expiresAtClaim = HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Expiration);
        var expiresAt = long.Parse(expiresAtClaim?.Value ?? "0");
        return Task.FromResult<IActionResult>(Ok(DateTime.UtcNow.Ticks < expiresAt ? identity.Name : "non"));
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

    [Authorize(Policy = "UserPolicy")]
    [HttpGet]
    [Route("get/user/info")]
    public async Task<IActionResult> GetUserInfoAsync(CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        var unitOfWork = unitOfWorkProvider.Get();
        
        var user = await unitOfWork.UsersRepository
            .GetAsync<User>(u => u.Where(t => t.Id == requestContext.UserId),
                requestContext.CancellationToken)
            .FirstOrDefaultAsync();
        if (user == null)
            
            // по идее юзер должен уже находится, раз уж мы на странице пользователя
            
            throw new NotImplementedException();
        return Json(new { address = user.Address, username = user.TelegramUsername, name = user.Name });
    }

    [Authorize(Policy = "UserPolicy")]
    [HttpPost]
    [Route("set/user/info/address")]
    public async Task<IActionResult> SetUserInfoAddressAsync(CancellationToken cancellationToken)
    {
        var form = await HttpContext.Request.ReadFormAsync(cancellationToken);
        
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        var unitOfWork = unitOfWorkProvider.Get();
        
        var user = await unitOfWork.UsersRepository
            .GetAsync<User>(u => u.Where(t => t.Id == requestContext.UserId),
                requestContext.CancellationToken)
            .FirstOrDefaultAsync();
        if (user == null)
            
            // по идее юзер должен уже находится, раз уж мы на странице пользователя
            
            throw new NotImplementedException();

        user.Address = form["address"].ToString();
        await unitOfWork.CommitAsync(requestContext.CancellationToken);
        return Json(user.Address);
    }
    
    [Authorize(Policy = "UserPolicy")]
    [HttpPost]
    [Route("set/user/info/name")]
    public async Task<IActionResult> SetUserInfoNameAsync(CancellationToken cancellationToken)
    {
        var form = await HttpContext.Request.ReadFormAsync(cancellationToken);
        
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        var unitOfWork = unitOfWorkProvider.Get();
        
        var user = await unitOfWork.UsersRepository
            .GetAsync<User>(u => u.Where(t => t.Id == requestContext.UserId),
                requestContext.CancellationToken)
            .FirstOrDefaultAsync();
        if (user == null)
            
            // по идее юзер должен уже находится, раз уж мы на странице пользователя
            
            throw new NotImplementedException();

        user.Name = form["name"].ToString();
        await unitOfWork.CommitAsync(requestContext.CancellationToken);
        return Json(user.Address);
    }

    [HttpGet]
    [Route("get/product/info")]
    public async Task<IActionResult> GetProductInfoNameAsync(CancellationToken cancellationToken)
    {
        var fullId = new
        {
            userId = int.Parse(HttpContext.Request.Headers["userId"].ToString()),
            marketId = int.Parse(HttpContext.Request.Headers["marketId"].ToString()),
            productId = int.Parse(HttpContext.Request.Headers["productId"].ToString())
        };
        
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        var unitOfWork = unitOfWorkProvider.Get();
        var product = await unitOfWork.ProductRepository
            .GetAsync(p => p
                .Where(x => x.ProductId == fullId.productId),
                requestContext.CancellationToken).FirstOrDefaultAsync();
        if (product == null)
            throw new NotImplementedException();
        var productData = new
        {
            title = product.Title,
            price = product.Price,
            remained = product.Remained
        };
        return Json(productData);
    }
}