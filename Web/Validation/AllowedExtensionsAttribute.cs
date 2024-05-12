using System.ComponentModel.DataAnnotations;

namespace Web.Validation;

public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly HashSet<string> extensions;
    
    public AllowedExtensionsAttribute(string[] extensions)
    {
        this.extensions = extensions.ToHashSet();
    }
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
        {
            return ValidationResult.Success;
        }
        
        var extension = Path.GetExtension(file.FileName).ToLower();
        return extensions.Contains(extension) 
            ? ValidationResult.Success 
            : new ValidationResult($"Wrong extension: {extension}. " +
                                   $"{string.Join(", ", extensions)} is only allowed.");
    }
}