using System.Security.Claims;
using Core.Objects;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ViewComponents;

namespace Web.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    
    public AccountController(
        IUnitOfWorkProvider unitOfWorkProvider,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.userManager = userManager;
        this.signInManager = signInManager;
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
        ViewBag.returnUrl = "/";
        return View(new LoginModel());
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginModel model, string returnUrl)
    {
        if (ModelState.IsValid)
        {
            await using var unitOfWork = unitOfWorkProvider.Get();

            var users = await unitOfWork.UsersRepository.GetAsync(
                r => r.Where(t => t.TelegramId == model.TelegramId), 
                CancellationToken.None);
            var user = users.FirstOrDefault();
            if (user == null)
            {
                unitOfWork.UsersRepository.Create(
                    new User
                    {
                        TelegramId = model.TelegramId,
                        TelegramUsername = model.TelegramUsername,
                        Name = model.Name,
                        PhoneNumber = model.PhoneNumber
                    });
            }
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }

    private async Task Authenticate(long telegramId)
    {
        var claims = new List<Claim>
        {
            new (ClaimsIdentity.DefaultNameClaimType, telegramId.ToString())
        };
        var id = new ClaimsIdentity(claims, 
            "ApplicationCookie", 
            ClaimsIdentity.DefaultNameClaimType, 
            ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}