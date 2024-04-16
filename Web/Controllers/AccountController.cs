﻿using System.Security.Claims;
using Core.Repositories.Users;
using Domain.Objects;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ViewComponents;

namespace Web.Controllers;

public class AccountController : Controller
{
    private readonly IUsersRepository usersRepository;
    
    public AccountController(IUsersRepository usersRepository)
    {
        this.usersRepository = usersRepository;
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await usersRepository.FindAsync(model.Email).ConfigureAwait(false);
            if (user != null)
            {
                await Authenticate(model.Email);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }
        return View(model);
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await usersRepository.FindAsync(model.Email).ConfigureAwait(false);
            if (user == null)
            {
                await usersRepository.CreateAsync(model.Email, model.Password);

                await Authenticate(model.Email);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }
        return View(model);
    }

    private async Task Authenticate(string userName)
    {
        var claims = new List<Claim>
        {
            new (ClaimsIdentity.DefaultNameClaimType, userName)
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