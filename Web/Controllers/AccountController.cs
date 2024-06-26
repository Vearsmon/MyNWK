﻿using System.Security.Claims;
using Core.Crypto;
using Core.Objects.Markets;
using Core.Objects.MyNwkUnitOfWork;
using Core.Objects.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ViewComponents;

namespace Web.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;
    private readonly ITgAuthService tgAuthService;

    public AccountController(
        IUnitOfWorkProvider unitOfWorkProvider,
        ITgAuthService tgAuthService)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.tgAuthService = tgAuthService;
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login()
    {
        var form = await HttpContext.Request.ReadFormAsync();
        var userMeta = ITgAuthService.KeysToUseInHash.ToDictionary(key => key, key => form[key].ToString());
        var hash = form["hash"].ToString();

        if (!tgAuthService.IsUserMetaValid(hash, userMeta))
        {
            return Ok();
        }
        
        await using var unitOfWork = unitOfWorkProvider.Get();

        var id = long.Parse(form["id"].ToString());
        var users = await unitOfWork.UsersRepository.GetAsync(
            r  => r.Where(t => t.TelegramId == id),
            CancellationToken.None);
        var user = users.FirstOrDefault();
        if (user == null)
        {
            var nameChunks = new List<string>(2);
            if (form["first_name"] != "undefined")
            {
                nameChunks.Add(form["first_name"]!);
            }
            if (form["last_name"] != "undefined")
            {
                nameChunks.Add(form["last_name"]!);
            }
            user = new User 
            { 
                TelegramId = id,
                TelegramUsername = form["username"].ToString(),
                Name = string.Join(' ', nameChunks)
            };
            unitOfWork.UsersRepository.Create(user);
            await unitOfWork.CommitAsync(CancellationToken.None);
            
            //TODO: вынести создание магазина

            var market = new Market
            {
                OwnerId = user.Id,
                Closed = false,
                Name = "shop" + user.Id,
                MarketInfo = new MarketInfo()
            };
            unitOfWork.MarketsRepository.Create(market);
            await unitOfWork.CommitAsync(CancellationToken.None);
        }
        
        await AuthenticateAsync(user.Id);
        return Ok();
    }

    private async Task AuthenticateAsync(long userId)
    {
        var claims = new List<Claim>
        {
            new (ClaimsIdentity.DefaultNameClaimType, userId.ToString()),
            new (ClaimTypes.Expiration, (DateTime.UtcNow + TimeSpan.FromDays(1)).Ticks.ToString()),
            new ("AllowUserActions", "AllowUserActions")
        };
        
        var id = new ClaimsIdentity(
            claims, 
            "ApplicationCookie", 
            ClaimsIdentity.DefaultNameClaimType, 
            ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(new ClaimsPrincipal(id));
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Baraholka");
    }
}