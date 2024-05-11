using System.Security.Claims;
using Core;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Users;
using Core.Services.Categories;
using Core.Services.Markets;
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

    [HttpPost]
    [Route("set/user/info/address")]
    public async Task<IActionResult> SetUserInfoAddressAsync(CancellationToken cancellationToken)
    {
        var form = await HttpContext.Request.ReadFormAsync();
        
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
    
    [HttpPost]
    [Route("set/user/info/name")]
    public async Task<IActionResult> SetUserInfoNameAsync(CancellationToken cancellationToken)
    {
        var form = await HttpContext.Request.ReadFormAsync();
        
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
}