using System.Text;
using Castle.Components.DictionaryAdapter.Xml;
using Core.Objects;
using Core.Objects.MyNwkUnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ViewComponents;

namespace Web.Areas.User.Controllers;

public class BaraholkaController : Controller
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;
    
    public BaraholkaController(IUnitOfWorkProvider unitOfWorkProvider)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View("~/Pages/Baraholka.cshtml");
    }
}