using Microsoft.EntityFrameworkCore;

using Testcontainers.PostgreSql;

using Lottery.DB.Context;
using Lottery.Integration.Test.Abstractions;
namespace Lottery.Integration.Test;


/// <summary>
/// Enables sharing the fixture between other related integration tests.
/// 
/// This allows us to re-use the same container for every all integration tests, 
/// which helps speed up the tests.
/// </summary>
[CollectionDefinition("Integration")]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
{ }

/// <summary>
/// A fixture providing access to the the high-level resources that integration 
/// tests require access to.
/// </summary>
public class IntegrationTestFixture : IAsyncLifetime
{
    private PostgreSqlContainer _container = null!;
    private string ConnectionString => _container.GetConnectionString();

    /// <summary>
    /// Creates a new <see cref="IntegrationTestContext"/> so that each test can
    /// get it's own resources, in order to prevent tests interfering with each other.
    /// </summary>
    public async Task<IntegrationTestContext> CreateTestContext()
    {
        var timeProvider = new FakeTimeProvider();
        var factory = new TestApplicationFactory(_container, timeProvider);

        return await IntegrationTestContext.CreateAsync(timeProvider, factory, ConnectionString);
    }

    public IntegrationTestContextBuilder TestContextBuilder()
        => new(_container);

    /// <summary>
    /// Called before every test collection that uses this fixture.
    /// 
    /// Creates a postgres DB in docker and deploys the lottery schema to it.
    /// /// </summary>
    async ValueTask IAsyncLifetime.InitializeAsync()
    {
        _container = new PostgreSqlBuilder("postgres:18")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithDatabase("lottery")
            .Build();

        await _container.StartAsync();

        using var migrationContext = new LotteryDBContext(
            new DbContextOptionsBuilder<LotteryDBContext>()
                .UseNpgsql(ConnectionString)
                .Options);

        await migrationContext.Database.MigrateAsync();
    }

    /// <summary>
    /// Called after every test collection that uses this fixture.
    /// </summary>
    /// <returns></returns>
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}

