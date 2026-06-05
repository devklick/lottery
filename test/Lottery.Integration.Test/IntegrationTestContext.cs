using Lottery.Api.Services.Options;
using Lottery.DB.Context;
using Lottery.DB.Entities.Idt;
using Lottery.Integration.Test.Abstractions;
using Lottery.Integration.Test.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Npgsql;

using Respawn;

using Testcontainers.PostgreSql;

namespace Lottery.Integration.Test;

/// <summary>
/// Provides each test case with it's own context.
/// </summary>
public class IntegrationTestContext : IAsyncDisposable
{
    /// <summary>
    /// An mockable TimeProvider
    /// </summary>
    public FakeTimeProvider TimeProvider { get; }

    /// <summary>
    /// A client to execute requests against in order to hit the Lottery API.
    /// </summary>
    public HttpClient Client { get; }

    private readonly TestApplicationFactory _factory;
    private readonly string _connectionString;
    private readonly LotteryDBContext _seedContext;
    private readonly bool _shouldSeed;

    public IntegrationTestContext(FakeTimeProvider timeProvider, TestApplicationFactory factory, string connectionString, bool seed = true)
    {
        TimeProvider = timeProvider;
        _factory = factory;
        Client = _factory.CreateClient();
        _connectionString = connectionString;
        _shouldSeed = seed;

        _seedContext = new LotteryDBContext(new DbContextOptionsBuilder<LotteryDBContext>()
            .UseNpgsql(connectionString)
            .Options);
    }

    public static async Task<IntegrationTestContext> CreateAsync(FakeTimeProvider timeProvider, TestApplicationFactory factory, string connectionString, bool seed = true)
    {
        var context = new IntegrationTestContext(timeProvider, factory, connectionString, seed);
        await context.SeedData();
        return context;
    }

    /// <summary>
    /// Sut the "current" time to a mocked value.
    /// </summary>
    /// <param name="dateTimeOffset"></param>
    public void SetCurrentTime(DateTimeOffset dateTimeOffset)
        => TimeProvider.UtcNow = dateTimeOffset;

    /// <summary>
    /// Seeds the basic data that's required by most (if not all) integration tests.
    /// </summary>
    private async Task SeedData()
    {
        if (!_shouldSeed) return;
        await _seedContext.Users.AddRangeAsync(TestUsers.ServiceUser, TestUsers.AppUser, TestUsers.GameAdminUser);
        await _seedContext.Roles.AddRangeAsync(TestUsers.ServiceRole, TestUsers.BasicRole, TestUsers.GameAdminRole);
        await _seedContext.UserRoles.AddRangeAsync(
            new AppUserRole { RoleId = TestUsers.ServiceRole.Id, UserId = TestUsers.ServiceUser.Id },
            new AppUserRole { RoleId = TestUsers.BasicRole.Id, UserId = TestUsers.AppUser.Id },
            new AppUserRole { RoleId = TestUsers.GameAdminRole.Id, UserId = TestUsers.GameAdminUser.Id }
        );

        await _seedContext.SaveChangesAsync();
    }

    /// <summary>
    /// Cleans up this context's resources. 
    /// 
    /// Should be called at the end of each test case.
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        await _seedContext.DisposeAsync();
        await _factory.DisposeAsync();
        Client.Dispose();

        if (!_shouldSeed) return;

        using var dbConnection = new NpgsqlConnection(_connectionString);

        await dbConnection.OpenAsync();

        var respawner = await Respawner.CreateAsync(
           dbConnection,
           new RespawnerOptions
           {
               DbAdapter = DbAdapter.Postgres,
               TablesToIgnore = ["__EFMigrationsHistory"],
           });

        await respawner.ResetAsync(dbConnection);
    }
}

public class IntegrationTestContextBuilder(PostgreSqlContainer container)
{
    private FakeTimeProvider _timeProvider = new();
    private PostgreSqlContainer _container = container;
    private bool _shouldSeed = false;
    private readonly ICollection<Action<IServiceCollection>> _serviceOverrides = [];


    public IntegrationTestContextBuilder WithTimeProvider(FakeTimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        return this;
    }

    public IntegrationTestContextBuilder WithContainer(PostgreSqlContainer container)
    {
        _container = container;
        return this;
    }

    public IntegrationTestContextBuilder WithServiceOverride(Action<IServiceCollection> serviceOverride)
    {
        _serviceOverrides.Add(serviceOverride);
        return this;
    }

    /// <summary>
    /// Whether or not the test context should seed the core test data.
    /// 
    /// Defaults to <c>false</c>.
    /// 
    /// Generally, the builder will be used within a test method, where the default
    /// fixture has already been built by the outer test class. As such, data has already 
    /// been seeded.
    /// 
    /// Note, if seeding is disabled, data teardown will also be disabled on this context.
    /// </summary>
    public IntegrationTestContextBuilder ShouldSeed(bool shouldSeed)
    {
        _shouldSeed = shouldSeed;
        return this;
    }

    public async Task<IntegrationTestContext> BuildAsync()
    {
        var factory = new TestApplicationFactory(_container, _timeProvider, _serviceOverrides);
        return await IntegrationTestContext.CreateAsync(_timeProvider, factory, _container.GetConnectionString(), _shouldSeed);
    }
}

public class IntegrationTestContextProvider
{
    /// <summary>
    /// Sets up the relevant test resources suitable for the vast majority of integration tests.
    /// </summary>
    public required IntegrationTestContext Default { get; init; }

    /// <summary>
    /// Allows custom configuration of the test context, including customizing 
    /// the services used by the Web API being tested.
    /// </summary>
    public required Func<IntegrationTestContextBuilder> CreateBuilder { get; init; }
}