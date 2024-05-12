using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web.Models.ModelBinders;

public class CheckBoxModelBinderAttribute : Attribute, IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }
        
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        var value = valueProviderResult.FirstValue;
        bindingContext.Model = value == "on";
        return Task.CompletedTask;
    }
}