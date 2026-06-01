using System.Collections;
using System.Net;
using System.Net.Http.Json;

using Lottery.Api.Models.Account.SignIn;
using Lottery.Api.Models.Entry.Create;
using Lottery.Api.Models.Entry.Edit;
using Lottery.Api.Models.Entry.Search;
using Lottery.Api.Models.Game.Create;
using Lottery.Api.Models.Game.Get;
using Lottery.Api.Models.Game.Result;
using Lottery.Api.Models.Game.Search;
using Lottery.Common.Models;
using Lottery.DB.Entities.Dbo;
using Lottery.DB.Entities.Ref;
using Lottery.Integration.Test.Data;

using Microsoft.AspNetCore.WebUtilities;

namespace Lottery.Integration.Test.E2E;

[Collection("Integration")]
public class EndToEndTest(ITestContextAccessor testContextAccessor, IntegrationTestFixture fixture)
{
    [Fact]
    public async Task EndToEndTest_OK()
    {
        await AdminSignIn();
        await CreateGame();
        await SignOut();
        await UserSignIn();
        var gameId = await SearchGames();
        var game = await GetGame(gameId);
        fixture.TimeProvider.UtcNow = game.StartTime;
        var entryId = await CreateEntry(gameId);
        await EditEntry(entryId);
        fixture.TimeProvider.UtcNow = game.CloseTime;
        await CantEnterClosedGame(gameId);
        fixture.TimeProvider.UtcNow = game.DrawTime;
        await UserCantResultGame(gameId);
        await SignOut();
        await AdminSignIn();
        await AdminResultGame(gameId);
        await SignOut();
        await UserSignIn();
        await VerifyEntryPrizes(entryId);
    }

    private async Task SignIn(string username, string password)
    {
        var requestBody = new SignInRequestBody
        {
            Username = username,
            Password = password
        };

        var response = await fixture.Client.PostAsJsonAsync(
            "/account/signIn",
            requestBody,
            testContextAccessor.Current.CancellationToken);

        response.EnsureSuccessStatusCode();
    }

    private async Task AdminSignIn()
        => await SignIn(TestUsers.GameAdminUserName, TestUsers.GameAdminUserPassword);

    private async Task UserSignIn()
        => await SignIn(TestUsers.AppUserName, TestUsers.AppUserPassword);

    private async Task CreateGame()
    {
        var requestBody = new CreateGameRequestBody
        {
            StartTime = fixture.TimeProvider.UtcNow.AddMinutes(5).UtcDateTime,
            CloseTime = fixture.TimeProvider.UtcNow.AddMinutes(10).UtcDateTime,
            DrawTime = fixture.TimeProvider.UtcNow.AddMinutes(15).UtcDateTime,
            MaxSelections = 50,
            Name = "Test Game",
            Prizes = [
                new () { Position = 1, NumberMatchCount = 5 },
                new () { Position = 2, NumberMatchCount = 4 },
                new () { Position = 3, NumberMatchCount = 3 },
                new () { Position = 4, NumberMatchCount = 2 },
                new () { Position = 5, NumberMatchCount = 1 }
            ],
            SelectionsRequiredForEntry = 5,
            State = ItemState.Enabled
        };

        var response = await fixture.Client.PostAsJsonAsync(
            "/game",
            requestBody,
            testContextAccessor.Current.CancellationToken);

        response.EnsureSuccessStatusCode();
    }

    private async Task SignOut()
    {
        var response = await fixture.Client.PostAsync(
            "/account/signOut",
            null,
            testContextAccessor.Current.CancellationToken);

        response.EnsureSuccessStatusCode();
    }

    private async Task<Guid> SearchGames()
    {
        var request = new SearchGamesRequestQuery
        {
            GameStatus = [GameStatus.Open, GameStatus.Future],
            Limit = 10,
            Page = 1,
            Name = "Test",
            SortBy = SearchGamesRequestQuery.SortCriteria.CloseTime,
            SortDirection = Api.Models.Common.SortDirection.Asc
        };

        var queryString = ToQueryString(request);
        var response = await fixture.Client.GetAsync(
            $"/game/search?{queryString}",
            testContextAccessor.Current.CancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var error = await response.Content.ReadAsStringAsync();
            Assert.Fail(error);
        }

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<Result<SearchGamesResponse>>();

        Assert.NotNull(body);
        Assert.NotNull(body.Value);
        Assert.Equal(ResultStatus.Ok, body.Status);
        Assert.Null(body.Errors);
        Assert.Equal(10, body.Value.Limit);
        Assert.Equal(1, body.Value.Page);
        Assert.Equal(1, body.Value.Total);
        Assert.Single(body.Value.Items);
        return body.Value.Items.First().Id;
    }

    private async Task<GetGameResponse> GetGame(Guid gameId)
    {
        var response = await fixture.Client.GetFromJsonAsync<Result<GetGameResponse>>(
            $"/game/{gameId}",
            testContextAccessor.Current.CancellationToken);

        Assert.NotNull(response);
        Assert.Equal(ResultStatus.Ok, response.Status);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Value);
        return response.Value;
    }

    private async Task<Guid> CreateEntry(Guid gameId)
    {
        var requestBody = new CreateEntryRequestBody
        {
            GameId = gameId,
            Selections = [
                new() { SelectionNumber = 11},
                new() { SelectionNumber = 12},
                new() { SelectionNumber = 13},
                new() { SelectionNumber = 14},
                new() { SelectionNumber = 15},
            ]
        };
        var response = await fixture.Client.PostAsJsonAsync(
            "/entry",
            requestBody,
            testContextAccessor.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<Result<CreateEntryResponse>>();
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.Ok, body.Status);
        Assert.Null(body.Errors);
        Assert.NotNull(body.Value);
        return body.Value.Id;
    }

    private async Task CantEnterClosedGame(Guid gameId)
    {
        var requestBody = new CreateEntryRequestBody
        {
            GameId = gameId,
            Selections = [
                new() { SelectionNumber = 31},
                new() { SelectionNumber = 32},
                new() { SelectionNumber = 33},
                new() { SelectionNumber = 34},
                new() { SelectionNumber = 35},
            ]
        };
        var response = await fixture.Client.PostAsJsonAsync(
            "/entry",
            requestBody,
            testContextAccessor.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<Result<CreateEntryResponse>>();
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.BadRequest, body.Status);
        Assert.NotNull(body.Errors);
        var error = Assert.Single(body.Errors);
        Assert.Equal($"Game {gameId} is closed", error.Message);
        Assert.Null(body.Value);
    }

    private async Task EditEntry(Guid entryId)
    {
        var requestBody = new EditEntryRequestBody
        {
            Selections = [
                new() { SelectionNumber = 21},
                new() { SelectionNumber = 22},
                new() { SelectionNumber = 23},
                new() { SelectionNumber = 24},
                new() { SelectionNumber = 25},
            ]
        };
        var response = await fixture.Client.PostAsJsonAsync(
            $"/entry/{entryId}/edit",
            requestBody,
            testContextAccessor.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<Result<CreateEntryResponse>>();
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.Ok, body.Status);
        Assert.Null(body.Errors);
        Assert.NotNull(body.Value);
    }

    private async Task UserCantResultGame(Guid gameId)
    {
        var request = new ResultGameRequestBody
        {
            WinningSelections = [
                new() { SelectionNumber = 1}, // 1 matching number, lowest prize
                new() { SelectionNumber = 2},
                new() { SelectionNumber = 3},
                new() { SelectionNumber = 4},
                new() { SelectionNumber = 5},
            ]
        };
        var response = await fixture.Client.PostAsync(
            $"/game/{gameId}/result",
            JsonContent.Create(request),
            testContextAccessor.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    private async Task AdminResultGame(Guid gameId)
    {
        var request = new ResultGameRequestBody
        {
            WinningSelections = [
                new() { SelectionNumber = 21}, // 1 matching number, lowest prize
                new() { SelectionNumber = 32},
                new() { SelectionNumber = 33},
                new() { SelectionNumber = 34},
                new() { SelectionNumber = 35},
            ]
        };
        var response = await fixture.Client.PostAsync(
            $"/game/{gameId}/result",
            JsonContent.Create(request),
            testContextAccessor.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<Result<ResultGameResponse>>();

        Assert.NotNull(body);
        Assert.Equal(ResultStatus.Ok, body.Status);
        Assert.Null(body.Errors);
        Assert.NotNull(body.Value);
    }

    private async Task VerifyEntryPrizes(Guid gameId)
    {
        var request = new SearchEntriesRequestQuery
        {
            GameId = gameId,
        };
        var response = await fixture.Client.GetFromJsonAsync<Result<SearchEntriesResponse>>(
            $"/entry?{ToQueryString(request)}",
            testContextAccessor.Current.CancellationToken);

        Assert.NotNull(response);
        Assert.Equal(ResultStatus.Ok, response.Status);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Value);
        var entry = Assert.Single(response.Value.Items);
        Assert.NotNull(entry.Prize);
        Assert.Equal(1, entry.Prize.NumberMatchCount);
        Assert.Equal(5, entry.Prize.Position);
    }

    public static string ToQueryString(object obj)
    {
        var values = new Dictionary<string, string?>();

        foreach (var property in obj.GetType().GetProperties())
        {
            var value = property.GetValue(obj);

            if (value is null)
            {
                continue;
            }

            // Treat strings as scalar values, not collections of chars
            if (value is string str)
            {
                values[property.Name] = str;
                continue;
            }

            if (value is IEnumerable enumerable)
            {
                var index = 0;

                foreach (var item in enumerable)
                {
                    values[$"{property.Name}[{index}]"] = item?.ToString();
                    index++;
                }

                continue;
            }

            values[property.Name] = value.ToString();
        }

        return QueryHelpers.AddQueryString(string.Empty, values);
    }
}