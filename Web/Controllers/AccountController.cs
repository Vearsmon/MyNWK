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
    public IActionResult Login()
    {
        ViewBag.returnUrl = "/";
        return View(new LoginModel());
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string returnUrl)
    {
        var form = await HttpContext.Request.ReadFormAsync();
        await using var unitOfWork = unitOfWorkProvider.Get();

        var users = await unitOfWork.UsersRepository.GetAsync(
            r => r.Where(t => t.TelegramId == long.Parse(form["id"].ToString())), 
            CancellationToken.None);
        var user = users.FirstOrDefault();
        
        var identityUser = new IdentityUser
        {
            UserName = form["username"].ToString(),
            Id = form["id"].ToString()
        };
        
        if (user == null)
        {
            var createdUser = new User
            {
                TelegramId = long.Parse(form["id"].ToString()),
                TelegramUsername = form["username"].ToString(),
                Name = form["name"].ToString()
            };
            
            unitOfWork.UsersRepository.Create(createdUser);
            
            var result = await userManager.CreateAsync(identityUser, "MasterPassword#0");
            if (result.Succeeded)
            {
                var res = await userManager.AddToRoleAsync(identityUser, "USER");
                var usrs = await userManager.GetUsersInRoleAsync("USER");
                var a = 1;
                await signInManager.SignInAsync(identityUser, false);
                return Redirect($"http://127.0.0.1:80/user/Profile");
            }
        }
        else
        {
            var usr = await userManager.GetUserAsync(HttpContext.User);
            if (usr == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await signInManager.SignOutAsync();
                HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
                var result = await signInManager.PasswordSignInAsync(identityUser.UserName, "MasterPassword#0", false, false);
            
                if (result.Succeeded)
                {
                    return Redirect($"http://127.0.0.1:80/user/Profile");
                }
            }
            else
                return Redirect($"http://127.0.0.1:80/Baraholka");
        }
        
        return Redirect($"http://127.0.0.1:80/user/Profile");
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
        return RedirectToAction("Index", "Baraholka");
    }
}