namespace Lottery.DB.Context;

using DotNetEnv.Configuration;

using Lottery.Common.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


public class LotteryDBContextFactory
    : IDesignTimeDbContextFactory<LotteryDBContext>
{
    public LotteryDBContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddDotNetEnv("../../.env")
            .Build();

        var connectionString = ConfigExtensions.GetConnectionString(config, "MIGRATION_DB_USER", "MIGRATION_DB_PASSWORD");

        var options = new DbContextOptionsBuilder<LotteryDBContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new LotteryDBContext(options);
    }
}