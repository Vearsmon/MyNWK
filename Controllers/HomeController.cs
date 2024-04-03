using Microsoft.AspNetCore.Mvc;

namespace MyNWK.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}