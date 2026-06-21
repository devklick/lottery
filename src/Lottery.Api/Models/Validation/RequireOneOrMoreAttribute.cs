using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.Validation;

[AttributeUsage(AttributeTargets.Class)]
public class RequireOneOrMoreAttribute(params string[] propertyNames) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return new ValidationResult("Expected a value, found null");
        var type = value.GetType();
        foreach (var propertyName in propertyNames)
        {
            var property = type.GetProperty(propertyName)
                ?? throw new Exception($"Invalid {nameof(RequireOneOrMoreAttribute)} usage: {propertyName} does not exist on {type.Name}");

            if (property.GetValue(value) is not null)
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult(
            $"At least one of the following properties must be provided: {string.Join(", ", propertyNames)}",
            propertyNames);
    }
}