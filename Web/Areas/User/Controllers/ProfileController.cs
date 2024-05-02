using Core.Objects;
using Core.Objects.MyNwkUnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.User.Controllers;

public class ProfileController : Controller
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;
    
    public ProfileController(IUnitOfWorkProvider unitOfWorkProvider)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
    }
    
    public async Task<IActionResult> Index()
    {
        await using var unitOfWork = unitOfWorkProvider.Get();
        var products = await unitOfWork.ProductRepository
            .GetAsync(
                _ => _,
                CancellationToken.None)
            .ConfigureAwait(false);
        return View("~/Areas/User/Views/Profile/Index.cshtml", products.AsQueryable());
    }
}