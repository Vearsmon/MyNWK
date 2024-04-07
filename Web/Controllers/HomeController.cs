using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class HomeController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return Content(User.Identity.Name);
    }
}