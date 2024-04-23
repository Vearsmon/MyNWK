using Core.Repositories.Products;
using Domain.Objects;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.User.Controllers;

[Area("User")]
public class HomeController : Controller
{
    private readonly ProductsRepository Products;
    
    public HomeController(ProductsRepository products)
    {
        Products = products;
    }
    public IActionResult Index()
    {
        return View(Products.GetProductEntities());
    }
}