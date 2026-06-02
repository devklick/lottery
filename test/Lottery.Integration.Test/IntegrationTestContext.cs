using Lottery.DB.Context;
using Lottery.DB.Entities.Idt;
using Lottery.Integration.Test.Abstractions;
using Lottery.Integration.Test.Data;

using Microsoft.EntityFrameworkCore;

using Npgsql;

using Respawn;

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

    public IntegrationTestContext(FakeTimeProvider timeProvider, TestApplicationFactory factory, string connectionString)
    {
        TimeProvider = timeProvider;
        _factory = factory;
        Client = _factory.CreateClient();
        _connectionString = connectionString;

        _seedContext = new LotteryDBContext(new DbContextOptionsBuilder<LotteryDBContext>()
            .UseNpgsql(connectionString)
            .Options);
    }

    public static async Task<IntegrationTestContext> CreateAsync(FakeTimeProvider timeProvider, TestApplicationFactory factory, string connectionString)
    {
        var context = new IntegrationTestContext(timeProvider, factory, connectionString);
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