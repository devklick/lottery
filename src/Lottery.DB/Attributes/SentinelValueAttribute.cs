namespace Lottery.DB.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SentinelValueAttribute(object value) : Attribute
{
    public object Value { get; } = value;
}