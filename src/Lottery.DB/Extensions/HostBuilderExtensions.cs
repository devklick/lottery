using Lottery.DB.Configuration;
using Lottery.DB.Context;
using Lottery.DB.Entities.Ref;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Npgsql;

namespace Lottery.DB.Extensions;

public static class HostBuilderExtensions
{
    public static IHostApplicationBuilder ConfigureEntityFramework<TContext>(this IHostApplicationBuilder builder, string usernameConfigKey, string passwordConfigKey)
        where TContext : LotteryDBContext
    {
        var connectionString = GetConnectionString(builder, usernameConfigKey, passwordConfigKey);
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.MapEnum<ItemState>();

        var dataSource = dataSourceBuilder.Build();

        var migrationSettings = builder.Configuration.GetSection(nameof(EFMigrationSettings));
        builder.Services.Configure<EFMigrationSettings>(migrationSettings);

        builder.Services.AddDbContext<TContext>(options => options.UseNpgsql(dataSource, options =>
        {
            var settings = migrationSettings.Get<EFMigrationSettings>()
                ?? throw new Exception("No EFMigrationSettings found");

            options.MigrationsHistoryTable(tableName: settings.TableName, schema: settings.SchemaName);
        }));

        return builder;
    }

    private static string GetConnectionString(IHostApplicationBuilder builder, string usernameConfigKey, string passwordConfigKey)
    {
        var connectionString = builder.Configuration.GetConnectionString("Default")
            ?? throw new Exception("No default connection string found");

        var csb = new NpgsqlConnectionStringBuilder(connectionString);

        // when running locally, most of the connection string is defined in appsettings, 
        // however password wil be stored more securely and added to configuration.
        var dbUser = builder.Configuration[usernameConfigKey];
        var dbPassword = builder.Configuration[passwordConfigKey];

        if (!string.IsNullOrWhiteSpace(dbPassword)) csb.Password = dbPassword;
        if (!string.IsNullOrWhiteSpace(dbUser)) csb.Username = dbUser;

        return csb.ConnectionString;
    }
}