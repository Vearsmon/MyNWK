using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Web.Service;

public class UserAreaAuthorization : IControllerModelConvention
{
    private readonly string area;
    private readonly string policy;

    public UserAreaAuthorization(string area, string policy)
    {
        this.area = area;
        this.policy = policy;
    }
    
    public void Apply(ControllerModel controller)
    {
        if (controller.Attributes.Any(attr =>
                attr is AreaAttribute
                && (attr as AreaAttribute).RouteValue.Equals(area, StringComparison.OrdinalIgnoreCase))
            || controller.RouteValues.Any(route => route.Key.Equals("area", StringComparison.OrdinalIgnoreCase) 
                                                   && route.Value.Equals(area, StringComparison.OrdinalIgnoreCase)))
        {
            controller.Filters.Add(new AuthorizeFilter(policy));
        }
    }
}