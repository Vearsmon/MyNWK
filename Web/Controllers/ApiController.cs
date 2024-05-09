using System.Security.Claims;
using Core.Objects.MyNwkUnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class ApiController : Controller
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;
    
    public ApiController(IUnitOfWorkProvider unitOfWorkProvider)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
    }
    
    [HttpGet]
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
}