using Lottery.Common.Extensions;
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
        var connectionString = ConfigExtensions.GetConnectionString(builder.Configuration, usernameConfigKey, passwordConfigKey);
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
}