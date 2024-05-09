using Core.Objects.MyNwkUnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize(Policy = "UserPolicy")]
public class ProfileController : Controller
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;
    
    public ProfileController(IUnitOfWorkProvider unitOfWorkProvider)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
    }
    
    public async Task<IActionResult> Index(string smth)
    {
        return View("~/Areas/User/Views/Index.cshtml");
    }
}