using Lottery.Api;
using Lottery.DB.Context;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Testcontainers.PostgreSql;

namespace Lottery.Integration.Test;

public class TestApplicationFactory(
    PostgreSqlContainer postgres,
    TimeProvider timeProvider,
    ICollection<Action<IServiceCollection>> serviceOverrides)
    : WebApplicationFactory<Program>
{
    public TestApplicationFactory(PostgreSqlContainer postgres, TimeProvider timeProvider)
     : this(postgres, timeProvider, [])
    { }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Remove(services.Single(s => s.ServiceType == typeof(DbContextOptions<LotteryDBContext>)));
            services.AddDbContext<LotteryDBContext>(options => options.UseNpgsql(postgres.GetConnectionString()));

            services.Remove(services.Single(s => s.ServiceType == typeof(TimeProvider)));
            services.AddSingleton(timeProvider);

            foreach (var serviceOverride in serviceOverrides)
            {
                serviceOverride(services);
            }
        });

        builder.ConfigureAppConfiguration((context, config) =>
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                // stub basic env vars
                ["LOTTERY_POSTGRES_DB"] = "lottery",
                ["LOTTERY_API_DB_USER"] = "mock-api-db-user",
                ["LOTTERY_API_DB_PASSWORD"] = "mock-api-db-pass",
                ["LOTTERY_RESULTS_DB_USER"] = "mock-results-db-user",
                ["LOTTERY_RESULTS_DB_PASSWORD"] = "mock-results-db-pass",
                ["LOTTERY_SYSTEM_ADMIN_PASSWORD"] = "mock-system-admin-pass",
                ["LOTTERY_GAME_ADMIN_PASSWORD"] = "mock-game-admin-pass",
            }));
    }
}