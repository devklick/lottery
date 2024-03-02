using System.ComponentModel.DataAnnotations;

using Lottery.Common.Extensions;

namespace Lottery.Api.Models.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UniqueValues : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;

        if (value.GetType().IsAssignableTo(typeof(ICollection<>)))
        {
            return new ValidationResult("Value does not represent a collection", [validationContext.MemberName!]);
        }

        var list = (List<object>)value;

        if (list.HasDuplicate(out _))
        {
            return new ValidationResult("Value does not represent a collection", [validationContext.MemberName!]);
        }

        return ValidationResult.Success;
    }
}