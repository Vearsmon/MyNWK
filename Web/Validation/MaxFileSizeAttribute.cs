using System.ComponentModel.DataAnnotations;

namespace Web.Validation;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly uint maxFileSizeInBytes;
    private readonly uint maxFileSizeInMb;

    public MaxFileSizeAttribute(uint maxFileSizeInBytes)
    {
        this.maxFileSizeInBytes = maxFileSizeInBytes;

        maxFileSizeInMb = maxFileSizeInBytes / 1024 / 1024;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
        {
            return ValidationResult.Success;
        }

        return file.Length <= maxFileSizeInBytes 
            ? ValidationResult.Success 
            : new ValidationResult($"File size should be less or equal than {maxFileSizeInMb} MiB");
    }
}