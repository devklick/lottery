using Microsoft.EntityFrameworkCore;

using Testcontainers.PostgreSql;

using Lottery.DB.Context;
using Lottery.DB.Entities.Idt;
using Microsoft.AspNetCore.Identity;
using Lottery.Integration.Test.Data;
using Lottery.Integration.Test.Abstractions;

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

        await context.Users.AddRangeAsync(TestUsers.ServiceUser, TestUsers.AppUser);
        await context.Roles.AddRangeAsync(TestUsers.ServiceRole, TestUsers.BasicRole);
        await context.UserRoles.AddRangeAsync(
            new AppUserRole { RoleId = TestUsers.ServiceRole.Id, UserId = TestUsers.ServiceUser.Id },
            new AppUserRole { RoleId = TestUsers.BasicRole.Id, UserId = TestUsers.AppUser.Id }
        );

        await context.SaveChangesAsync();

        var user = await context.Users.SingleOrDefaultAsync(u => u.UserName == TestUsers.AppUser.UserName);
        Assert.NotNull(user);



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
