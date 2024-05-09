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
    
    [HttpPost]
    public async Task<IActionResult> Index(string id, string username, string name)
    {
        if (long.TryParse(id, out var _))
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://127.0.0.1");
            var values = new Dictionary<string, string>
            {
                { "id", id },
                { "username", username },
                { "name", name }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync($"/Account/Login/", content);

            var responseString = await response.Content.ReadAsStringAsync();
            return Redirect($"/user/Profile");
        }
        return View();
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