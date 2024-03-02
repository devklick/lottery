namespace Lottery.Repository.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class SqlTableCheckConstraintAttribute(string constraintName, string sql) : Attribute
{
    /// <summary>
    /// The name of the SQL constraint. Note that it will be prefixed and suffixed to conform to the database models naming conventions. For example, "ch_[schema]_[table]_[ConstraintName]."
    /// </summary>
    public string ConstraintName { get; } = constraintName;
    public string Sql { get; } = sql;
}