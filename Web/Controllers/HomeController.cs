using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Redirect($"http://127.0.0.1:80/Baraholka");
    }
}