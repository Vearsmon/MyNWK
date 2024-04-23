using System.Security.Claims;
using Core.Repositories.Users;
using Domain.Objects;
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
    private readonly IUsersRepository usersRepository;
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    
    public AccountController(
        IUsersRepository usersRepository,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        this.usersRepository = usersRepository;
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
            var user = await usersRepository.FindAsync(model.TelegramId);

            if (user == null)
            {
                await usersRepository.CreateAsync(model.TelegramId,
                    model.TelegramUsername, model.Name, model.PhoneNumber);
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