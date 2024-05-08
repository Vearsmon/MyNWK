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
    private readonly UserManager<IdentityUser> userManager;
    
    public BaraholkaController(IUnitOfWorkProvider unitOfWorkProvider,
        UserManager<IdentityUser> userManager)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.userManager = userManager;
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(string id, string username, string name)
    {
        if (long.TryParse(id, out var _))
        {
            var client = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "id", id },
                { "username", username },
                { "name", name }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync($"http://127.0.0.1:80/Account/Login/", content);

            var responseString = await response.Content.ReadAsStringAsync();
            return Redirect($"http://127.0.0.1:80/user/Profile");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Check()
    {
        var check = HttpContext.Request.Form["check"].ToString();
        if (check == "true")
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var bytesUser = Encoding.UTF8.GetBytes($"{user.Id}");
                await HttpContext.Response.Body.WriteAsync(bytesUser);
                return new EmptyResult();
            }
        }
        var bytes = Encoding.UTF8.GetBytes($"non");
        await HttpContext.Response.Body.WriteAsync(bytes);
        return new EmptyResult();
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        await using var unitOfWork = unitOfWorkProvider.Get();
        var products = await unitOfWork.ProductRepository
            .GetAsync(
                _ => _,
                CancellationToken.None)
            .ConfigureAwait(false);
        return View("~/Views/Baraholka/Index.cshtml");
    }
}