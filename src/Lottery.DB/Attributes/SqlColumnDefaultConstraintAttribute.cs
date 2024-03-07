namespace Lottery.DB.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SqlColumnDefaultConstraintAttribute(object defaultValue, bool isSqlCommand = false) : Attribute
{
    public object DefaultValue { get; } = defaultValue;
    public bool IsSqlCommand { get; set; } = isSqlCommand;
}