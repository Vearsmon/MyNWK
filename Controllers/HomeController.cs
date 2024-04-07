using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyNWK.Controllers;

public class HomeController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return Content(User.Identity.Name);
    }
}