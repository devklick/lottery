namespace Lottery.Common.Extensions;

using Microsoft.Extensions.Configuration;

using Npgsql;

public static class ConfigExtensions
{
    public static string GetConnectionString(this IConfiguration config, string usernameConfigKey, string passwordConfigKey)
    {
        var connectionString = config.GetConnectionString("Default")
            ?? throw new Exception("No default connection string found");

        var csb = new NpgsqlConnectionStringBuilder(connectionString);

        // when running locally, most of the connection string is defined in appsettings, 
        // however password wil be stored more securely and added to configuration.
        var dbUser = config[usernameConfigKey];
        var dbPassword = config[passwordConfigKey];

        if (!string.IsNullOrWhiteSpace(dbPassword)) csb.Password = dbPassword;
        if (!string.IsNullOrWhiteSpace(dbUser)) csb.Username = dbUser;

        return csb.ConnectionString;
    }
}