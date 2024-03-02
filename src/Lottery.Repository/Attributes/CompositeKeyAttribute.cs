namespace Lottery.Repository.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CompositeKeyAttribute(string firstColumnName, params string[] otherColumnNames) : Attribute
{
    public List<string> ColumnNames { get; } = [firstColumnName, .. otherColumnNames];
}