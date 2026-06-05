namespace Lottery.Integration.Test.Game;

# pragma warning disable xUnit1041
[Collection("Integration")]
public class GameTest(ITestContextAccessor testContextAccessor, IntegrationTestFixture fixture) : IntegrationTestBase(testContextAccessor, fixture)
{
    [Fact]
    public async Task Games_UnauthenticatedUser_OK()
    {
        var response = await TestContext.Default.Client.GetAsync("/game/search", CancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
#pragma warning restore xUnit1041