namespace Lottery.DB.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SqlTableUniqueIndexAttribute(params string[] propertyNames) : Attribute
{
    /// <summary>.
    /// The names of the properties to be included in the table level unique constraint
    /// </summary>
    public string[] PropertyNames { get; set; } = propertyNames;
    public string? ConstraintName { get; set; }
}