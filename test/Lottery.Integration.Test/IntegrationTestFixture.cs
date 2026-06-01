using Microsoft.EntityFrameworkCore;

using Testcontainers.PostgreSql;

using Lottery.DB.Context;
using Lottery.DB.Entities.Idt;
using Microsoft.AspNetCore.Identity;
using Lottery.Integration.Test.Data;
using Lottery.Integration.Test.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Lottery.Integration.Test;


public class IntegrationTestFixture : IAsyncLifetime
{
    private PostgreSqlContainer _container = null!;

    public TestApplicationFactory Factory { get; private set; } = null!;
    public HttpClient Client { get; private set; } = null!;
    public string ConnectionString => _container.GetConnectionString();
    public FakeTimeProvider TimeProvider { get; private set; } = new FakeTimeProvider
    {
        UtcNow = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero)
    };

    public LotteryDBContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LotteryDBContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new LotteryDBContext(options);
    }

    async ValueTask IAsyncLifetime.InitializeAsync()
    {
        _container = new PostgreSqlBuilder("postgres:18")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        await _container.StartAsync();

        await using var context = CreateContext();

        await context.Database.MigrateAsync();

        await context.Users.AddRangeAsync(TestUsers.ServiceUser, TestUsers.AppUser, TestUsers.GameAdminUser);
        await context.Roles.AddRangeAsync(TestUsers.ServiceRole, TestUsers.BasicRole, TestUsers.GameAdminRole);
        await context.UserRoles.AddRangeAsync(
            new AppUserRole { RoleId = TestUsers.ServiceRole.Id, UserId = TestUsers.ServiceUser.Id },
            new AppUserRole { RoleId = TestUsers.BasicRole.Id, UserId = TestUsers.AppUser.Id },
            new AppUserRole { RoleId = TestUsers.GameAdminRole.Id, UserId = TestUsers.GameAdminUser.Id }
        );

        await context.SaveChangesAsync();

        Factory = new TestApplicationFactory(_container, TimeProvider);

        Client = Factory.CreateClient();
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        Client.Dispose();
        await Factory.DisposeAsync();
        await _container.DisposeAsync();
    }
}

[CollectionDefinition("Integration")]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
{ }
