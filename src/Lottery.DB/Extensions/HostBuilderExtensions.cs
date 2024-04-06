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
    public static IHostApplicationBuilder ConfigureEntityFramework<TContext>(this IHostApplicationBuilder builder)
        where TContext : LotteryDBContext
    {
        var connectionString = GetConnectionString(builder);
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.MapEnum<ItemState>();

        builder.Services.Configure<EFMigrationSettings>(
            builder.Configuration.GetSection(nameof(EFMigrationSettings)));

        builder.Services.AddDbContext<TContext>(options => options.UseNpgsql(dataSourceBuilder.Build(), options =>
        {
            var settings = builder.Configuration.GetSection(nameof(EFMigrationSettings)).Get<EFMigrationSettings>()
                ?? throw new Exception("No EFMigrationSettings found");

            options.MigrationsHistoryTable(tableName: settings.TableName, schema: settings.SchemaName);
        }));

        return builder;
    }

    private static string GetConnectionString(IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Default")
            ?? throw new Exception("No default connection string found");

        var dbPassword = builder.Configuration["ConnectionStrings:Default:Password"];

        if (!string.IsNullOrWhiteSpace(dbPassword))
        {
            connectionString = connectionString.TrimEnd();
            connectionString += $";Password={dbPassword};";
        }
        return connectionString;
    }
}