namespace Lottery.Repository.Configuration;

public class EFMigrationSettings
{
    public required string TableName { get; set; }
    public required string SchemaName { get; set; }
}