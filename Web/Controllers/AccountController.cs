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
        if (!ModelState.IsValid) return View(model);
        await using var unitOfWork = unitOfWorkProvider.Get();

        var users = await unitOfWork.UsersRepository.GetAsync(
            r => r.Where(t => t.TelegramId == model.TelegramId), 
            CancellationToken.None);
        var user = users.FirstOrDefault();
        
        var identityUser = new IdentityUser
        {
            UserName = model.TelegramUsername,
            Id = model.TelegramId.ToString()
        };
        
        if (user == null)
        {
            var createdUser = new User
            {
                TelegramId = model.TelegramId,
                TelegramUsername = model.TelegramUsername,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber
            };
            
            unitOfWork.UsersRepository.Create(createdUser);
            
            var result = await userManager.CreateAsync(identityUser, "MasterPassword#0");
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(identityUser, false);
                return RedirectToAction("Index", "Profile");
            }
        }
        else
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await signInManager.SignOutAsync();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
            var result = await signInManager.PasswordSignInAsync(identityUser.UserName, "MasterPassword#0", false, false);
            
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Profile");
            }
        }
        
        return RedirectToAction("Index", "Profile");
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