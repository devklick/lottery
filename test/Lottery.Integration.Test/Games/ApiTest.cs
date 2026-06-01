namespace Lottery.Integration.Test.Games;

# pragma warning disable xUnit1041
[Collection("Integration")]
public class ApiTest(ITestContextAccessor testContextAccessor, IntegrationTestFixture fixture)
{
    [Fact]
    public async Task Games_UnauthenticatedUser_OK()
    {
        var response = await fixture.Client.GetAsync("/game/search", testContextAccessor.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
#pragma warning restore xUnit1041