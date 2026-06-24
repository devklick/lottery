using System.Net;
using System.Net.Http.Json;

using Lottery.Api.Models.Account.SignIn;
using Lottery.Integration.Test.Data;

namespace Lottery.Integration.Test.Account;

[Collection("Integration")]
public class SignOutTest(ITestContextAccessor testContextAccessor, IntegrationTestFixture fixture) : IntegrationTestBase(testContextAccessor, fixture)
{
    [Fact]
    public async Task SignUp_MissingField_BadRequest()
    {

        var request = new SignInRequestBody
        {
            UsernameOrEmail = TestUsers.AppUser1.UserName!,
            Password = TestUsers.AppUser1Password,
        };

        var signInResponse = await TestContext.Default.Client.PostAsync(
            "/account/signIn",
            JsonContent.Create(request),
            CancellationToken);

        signInResponse.EnsureSuccessStatusCode();

        // check protected endpoint
        var entriesResponse = await TestContext.Default.Client.GetAsync(
            "/entry",
            CancellationToken);

        Assert.NotEqual(HttpStatusCode.Forbidden, entriesResponse.StatusCode);

        var signOutResponse = await TestContext.Default.Client.PostAsync(
            "/account/signOut",
            null,
            CancellationToken);

        signOutResponse.EnsureSuccessStatusCode();

        // check protected endpoint
        var entriesResponse2 = await TestContext.Default.Client.PostAsync(
            "/entry",
            null,
            CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, entriesResponse2.StatusCode);
    }

}