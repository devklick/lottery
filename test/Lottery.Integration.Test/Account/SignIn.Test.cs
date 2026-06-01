using System.Net;
using System.Net.Http.Json;

using Lottery.Api.Models.Account.SignIn;
using Lottery.Api.Models.Common;
using Lottery.Common.Models;
using Lottery.Integration.Test.Data;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Integration.Test.Account;


#pragma warning disable xUnit1041
[Collection("Integration")]
public class SignInTest(ITestContextAccessor testContextAccessor, IntegrationTestFixture fixture)
{
    private CancellationToken CancellationToken => testContextAccessor.Current.CancellationToken;

    [Fact]
    public async Task SignIn_UsernameMissing_BadRequest()
    {
        var request = new SignInRequestBody
        {
            Username = null!,
            Password = "value",
        };

        var response = await fixture.Client.PostAsync(
            "/account/signIn",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(CancellationToken);

        Assert.NotNull(body);
        Assert.Contains("Username", body.Errors.Keys);
        Assert.Equal("The Username field is required.", body.Errors["Username"][0]);
    }

    [Fact]
    public async Task SignIn_PasswordMissing_BadRequest()
    {
        var request = new SignInRequestBody
        {
            Username = "value",
            Password = null!,
        };

        var response = await fixture.Client.PostAsync(
            "/account/signIn",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(CancellationToken);

        Assert.NotNull(body);
        Assert.Contains("Password", body.Errors.Keys);
        Assert.Equal("The Password field is required.", body.Errors["Username"][0]);
    }

    [Fact]
    public async Task SignIn_UsernameNotFound()
    {
        var request = new SignInRequestBody
        {
            Username = "value",
            Password = "value",
        };
        var response = await fixture.Client.PostAsync(
            "/account/signIn",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<Result<SignInResponse>>(CancellationToken);
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.NotFound, body.Status);
        Assert.Null(body.Value);
        Assert.NotNull(body.Errors);
        var error = Assert.Single(body.Errors);
        Assert.Equal("Username not registered", error.Message);
    }

    [Fact]
    public async Task SignIn_WrongPassword()
    {
        var request = new SignInRequestBody
        {
            Username = TestUsers.AppUser.UserName!,
            Password = "wrong",
        };
        var response = await fixture.Client.PostAsync(
            "/account/signIn",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<Result<SignInResponse>>(CancellationToken);
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.NotAuthenticated, body.Status);
        Assert.Null(body.Value);
        Assert.NotNull(body.Errors);
        var error = Assert.Single(body.Errors);
        Assert.Equal("Invalid sign in credentials", error.Message);
    }

    [Fact]
    public async Task SignIn_BasicUser_Success()
    {
        fixture.TimeProvider.UtcNow = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

        var request = new SignInRequestBody
        {
            Username = TestUsers.AppUser.UserName!,
            Password = TestUsers.AppUserPassword,
        };
        var response = await fixture.Client.PostAsync(
            "/account/signIn",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<Result<SignInResponse>>(CancellationToken);
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.Ok, body.Status);
        Assert.NotNull(body.Value);
        // The magic 14 here comes from cookie options and should ideally be controlled during test
        Assert.Equal(fixture.TimeProvider.UtcNow.AddDays(14), body.Value.SessionExpiry);
        Assert.Equal(UserType.Basic, body.Value.UserType);
        Assert.Null(body.Errors);
    }
}
#pragma warning restore xUnit1041