using Core.Objects;
using Core.Objects.MyNwkUnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.User.Controllers;

public class BaraholkaController : Controller
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;
    
    public BaraholkaController(IUnitOfWorkProvider unitOfWorkProvider)
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
        return View("~/Views/Baraholka/Index.cshtml", products.AsQueryable());
    }
}