using Lottery.DB.Configuration;
using Lottery.DB.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lottery.DB.Extensions;

public static class HostBuilderExtensions
{
    public static IHostApplicationBuilder ConfigureEntityFramework<TContext>(this IHostApplicationBuilder builder)
        where TContext : LotteryDBContext
    {
        builder.Services.Configure<EFMigrationSettings>(
            builder.Configuration.GetSection(nameof(EFMigrationSettings)));

        builder.Services.AddDbContext<TContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("Default")
                ?? throw new Exception("No default connection string found");

            var dbPassword = builder.Configuration["ConnectionStrings:Default:Password"];

            if (!string.IsNullOrWhiteSpace(dbPassword))
            {
                connectionString = connectionString.TrimEnd();
                connectionString += $";Password={dbPassword};";
            }
            options.UseNpgsql(connectionString, sqlOps =>
            {
                var settings = builder.Configuration.GetSection(nameof(EFMigrationSettings)).Get<EFMigrationSettings>()
                    ?? throw new Exception("No EFMigrationSettings found");

                sqlOps.MigrationsHistoryTable(tableName: settings.TableName, schema: settings.SchemaName);
            });
        });

        return builder;
    }
}