using System.ComponentModel.DataAnnotations;
using Core;
using Core.Helpers;
using Core.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models.ViewComponents;

namespace Web.Controllers;

[Authorize(Policy = "UserPolicy")]
[Route("orders")]
public class OrdersController : Controller
{
    private readonly IOrdersService ordersService;

    public OrdersController(IOrdersService ordersService)
    {
        this.ordersService = ordersService;
    }
    
    [HttpGet]
    [Route("confirm")]
    public async Task<IActionResult> ConfirmAsync(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        await ordersService.ConfirmAsync(requestContext, orderId);
        return Redirect("/Profile");
    }

    [HttpGet]
    [Route("cancel")]
    public async Task<IActionResult> CancelAsync(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var requestContext = RequestContextBuilder.Build(HttpContext, cancellationToken);
        await ordersService.CancelAsync(requestContext, orderId);
        return Redirect("/Profile");
    }
}