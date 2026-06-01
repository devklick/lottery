using System.Net;
using System.Net.Http.Json;

using Lottery.Api.Models.Account.SignIn;
using Lottery.Integration.Test.Data;

namespace Lottery.Integration.Test.Account;

[Collection("Integration")]
public class SignOutTest(ITestContextAccessor testContextAccessor, IntegrationTestFixture fixture)
{
    private CancellationToken CancellationToken => testContextAccessor.Current.CancellationToken;

    [Fact]
    public async Task SignUp_MissingField_BadRequest()
    {

        var request = new SignInRequestBody
        {
            Username = TestUsers.AppUser.UserName!,
            Password = TestUsers.AppUserPassword,
        };

        var signInResponse = await fixture.Client.PostAsync(
            "/account/signIn",
            JsonContent.Create(request),
            CancellationToken);

        signInResponse.EnsureSuccessStatusCode();

        // check protected endpoint
        var entriesResponse = await fixture.Client.GetAsync(
            "/entry",
            CancellationToken);

        Assert.NotEqual(HttpStatusCode.Forbidden, entriesResponse.StatusCode);

        var signOutResponse = await fixture.Client.PostAsync(
            "/account/signOut",
            null,
            CancellationToken);

        signOutResponse.EnsureSuccessStatusCode();

        // check protected endpoint
        var entriesResponse2 = await fixture.Client.PostAsync(
            "/entry",
            null,
            CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, entriesResponse2.StatusCode);
    }

}