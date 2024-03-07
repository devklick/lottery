namespace Lottery.DB.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SqlTableIndexAttribute(params string[] propertyNames) : Attribute
{
    public bool IsUnique { get; set; }
    public string[] PropertyNames { get; } = propertyNames;
}