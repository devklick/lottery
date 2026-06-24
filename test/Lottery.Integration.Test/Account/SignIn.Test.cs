using System.Net;
using System.Net.Http.Json;

using Lottery.Api.Models.Account.SignIn;
using Lottery.Api.Models.Common;
using Lottery.Common.Models;
using Lottery.Integration.Test.Data;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Integration.Test.Account;


#pragma warning disable xUnit1041
[Collection("Integration")]
public class SignInTest(ITestContextAccessor testContextAccessor, IntegrationTestFixture fixture) : IntegrationTestBase(testContextAccessor, fixture)
{
    [Theory]
    [InlineData(null, "value", "Username")]
    [InlineData("value", null, "Password")]
    public async Task SignIn_MissingField_BadRequest(string? username, string? password, string field)
    {
        var request = new SignInRequestBody
        {
            UsernameOrEmail = username!,
            Password = password!,
        };

        var response = await TestContext.Default.Client.PostAsync(
            "/account/signIn",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(CancellationToken);

        Assert.NotNull(body);
        Assert.Contains(field, body.Errors.Keys);
        Assert.Equal($"The {field} field is required.", body.Errors[field][0]);
    }

    [Fact]
    public async Task SignIn_UsernameNotFound()
    {
        var request = new SignInRequestBody
        {
            UsernameOrEmail = "value",
            Password = "value",
        };
        var response = await TestContext.Default.Client.PostAsync(
            "/account/signIn",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<Result<SignInResponse>>(CancellationToken);
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.NotFound, body.Status);
        Assert.Null(body.Value);
        Assert.NotNull(body.Messages);
        var error = Assert.Single(body.Messages);
        Assert.Equal("Username not registered", error.Value);
    }

    [Fact]
    public async Task SignIn_WrongPassword()
    {
        var request = new SignInRequestBody
        {
            UsernameOrEmail = TestUsers.AppUser1.UserName!,
            Password = "wrong",
        };
        var response = await TestContext.Default.Client.PostAsync(
            "/account/signIn",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<Result<SignInResponse>>(CancellationToken);
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.NotAuthenticated, body.Status);
        Assert.Null(body.Value);
        Assert.NotNull(body.Messages);
        var error = Assert.Single(body.Messages);
        Assert.Equal("Invalid sign in credentials", error.Value);
    }

    [Fact]
    public async Task SignIn_BasicUser_Success()
    {
        var cookieExpiryTimespan = TimeSpan.FromDays(1);

        await using var context = await TestContext
            .CreateBuilder()
            .WithServiceOverride(services =>
                services.PostConfigure<CookieAuthenticationOptions>(options =>
                    options.ExpireTimeSpan = cookieExpiryTimespan))
            .BuildAsync();

        context.SetCurrentTime(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));

        var request = new SignInRequestBody
        {
            UsernameOrEmail = TestUsers.AppUser1.UserName!,
            Password = TestUsers.AppUser1Password,
        };

        var response = await context.Client.PostAsync(
            "/account/signIn",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<Result<SignInResponse>>(CancellationToken);
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.Ok, body.Status);
        Assert.NotNull(body.Value);
        Assert.Equal(context.TimeProvider.UtcNow.Add(cookieExpiryTimespan), body.Value.SessionExpiry);
        Assert.Equal(UserType.Basic, body.Value.UserType);
        Assert.Null(body.Messages);
    }
}
#pragma warning restore xUnit1041