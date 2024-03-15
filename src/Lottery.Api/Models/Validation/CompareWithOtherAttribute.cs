using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.Validation;

public enum ComparisonType
{
    Equal,
    NotEqual,
    LessThan,
    LessThanOrEqual,
    GreaterThan,
    GreaterThanOrEqual
}

public class CompareWithOtherAttribute(ComparisonType comparison, string otherPropertyName) : ValidationAttribute
{
    private readonly string _otherPropertyName = otherPropertyName;
    private readonly ComparisonType _comparison = comparison;

    public override bool RequiresValidationContext => true;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;


        if (value is not IComparable valueA)
        {
            throw new NotSupportedException($"Property {validationContext.DisplayName} cannot be compared");
        }

        var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName)
            ?? throw new KeyNotFoundException($"Property {_otherPropertyName} does not exist on {validationContext.ObjectType.Name}");

        var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

        if (otherValue == null) return ValidationResult.Success;

        if (otherValue is not IComparable valueB)
        {
            throw new NotSupportedException($"Property {_otherPropertyName} cannot be compared");
        }

        return Compare(valueA, _comparison, valueB)
            ? ValidationResult.Success
            : new ValidationResult(
                $"{validationContext.MemberName} must be less than {_otherPropertyName}",
                [validationContext.MemberName ?? ""]);
    }

    private static bool Compare(IComparable a, ComparisonType comparison, IComparable? b) => comparison switch
    {
        ComparisonType.Equal => a.CompareTo(b) == 0,
        ComparisonType.NotEqual => a.CompareTo(b) != 0,
        ComparisonType.LessThan => a.CompareTo(b) < 0,
        ComparisonType.LessThanOrEqual => a.CompareTo(b) <= 0,
        ComparisonType.GreaterThan => a.CompareTo(b) > 0,
        ComparisonType.GreaterThanOrEqual => a.CompareTo(b) >= 0,
        _ => throw new NotImplementedException($"ComparisonType {comparison} not implemented")
    };
}