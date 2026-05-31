using Microsoft.EntityFrameworkCore;

using Testcontainers.PostgreSql;

using Lottery.DB.Context;

namespace Lottery.Integration.Test;


public class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    public PostgreSqlFixture()
    {
        _container = new PostgreSqlBuilder("postgres:17")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    public string ConnectionString => _container.GetConnectionString();

    public LotteryDBContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LotteryDBContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new LotteryDBContext(options);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        await using var context = CreateContext();

        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<PostgreSqlFixture>
{
}
