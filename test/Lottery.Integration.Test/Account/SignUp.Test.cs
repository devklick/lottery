using System.Net;
using System.Net.Http.Json;

using Lottery.Api.Models.Account.SignUp;
using Lottery.Common.Models;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Integration.Test.Account;

[Collection("Integration")]
public class SignUpTest(ITestContextAccessor testContextAccessor, IntegrationTestFixture fixture) : IntegrationTestBase(testContextAccessor, fixture)
{

    [Theory]
    [InlineData(null, "value", "test@test.com", "Username")]
    [InlineData("value", null, "test@test.com", "Password")]
    [InlineData("value", "value", null, "Email")]
    public async Task SignUp_MissingField_BadRequest(string? username, string? password, string? email, string field)
    {
        var request = new SignUpRequestBody
        {
            Username = username!,
            Password = password!,
            Email = email!
        };

        var response = await TestContext.Client.PostAsync(
            "/account/signUp",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(CancellationToken);

        Assert.NotNull(body);
        Assert.Contains(field, body.Errors.Keys);
        Assert.Equal($"The {field} field is required.", body.Errors[field][0]);
    }

    [Fact]
    public async Task SignUp_NoRoleSpecified_BasicUserCreated_Success()
    {
        var request = new SignUpRequestBody
        {
            Username = "test",
            Password = "Passw0rd*1",
            Email = "test@test.com"
        };

        var response = await TestContext.Client.PostAsync(
            "/account/signUp",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<Result<SignUpResponse>>(CancellationToken);
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.Ok, body.Status);
        Assert.Null(body.Errors);
    }
}