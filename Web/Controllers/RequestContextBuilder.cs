using Core;

namespace Web.Controllers;

public static class RequestContextBuilder
{
    public static RequestContext Build(
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var identity = httpContext.User.Identity;
        var userId = identity?.Name;
        return new RequestContext
        {
            CancellationToken = cancellationToken,
            UserId = userId is null
                ? null
                : int.Parse(userId)
        };
    }
}