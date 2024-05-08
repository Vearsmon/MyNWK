using System.Text;
using Core.Objects.MyNwkUnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class ApiController : Controller
{
    private readonly IUnitOfWorkProvider unitOfWorkProvider;
    private readonly UserManager<IdentityUser> userManager;
    
    public ApiController(IUnitOfWorkProvider unitOfWorkProvider,
        UserManager<IdentityUser> userManager)
    {
        this.unitOfWorkProvider = unitOfWorkProvider;
        this.userManager = userManager;
    }
    
    public async Task<IActionResult> GetUser()
    {
        var check = HttpContext.Request.Form["check"].ToString();
        if (check == "true")
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var originalBody = HttpContext.Response.Body;

                try {
                    using (var memStream = new MemoryStream())
                    {
                        var bytesUser = Encoding.UTF8.GetBytes($"{user.Id}");
                        await memStream.WriteAsync(bytesUser);
                        HttpContext.Response.Body = memStream;

                        memStream.Position = 0;
                        string responseBody = new StreamReader(memStream).ReadToEnd();

                        memStream.Position = 0;
                        await memStream.CopyToAsync(originalBody);
                    }

                } finally {
                    HttpContext.Response.Body = originalBody;
                }
                
                
                // var bytesUser = Encoding.UTF8.GetBytes($"{user.Id}");
                // await HttpContext.Response.Body.WriteAsync(bytesUser);
                return new EmptyResult();
            }
        }
        var bytes = Encoding.UTF8.GetBytes($"non");
        await HttpContext.Response.Body.WriteAsync(bytes);
        return new EmptyResult();
    }
}