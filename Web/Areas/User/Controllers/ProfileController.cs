using Core.Objects.MyNwkUnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Area("User")]
public class ProfileController : Controller
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;
    private readonly UserManager<IdentityUser> userManager;
    
    public ProfileController(IUnitOfWorkProvider unitOfWorkProvider,
        UserManager<IdentityUser> userManager)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.userManager = userManager;
    }
    
    public async Task<IActionResult> Index(string smth)
    {
        var user = await userManager.GetUserAsync(HttpContext.User);
        return View("~/Areas/User/Views/Index.cshtml");
    }
}