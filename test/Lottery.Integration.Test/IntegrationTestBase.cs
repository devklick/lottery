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

namespace Lottery.Integration.Test;

/// <summary>
/// Base class to be inherited by any class representing a collection of integration tests.
/// 
/// Is initialized before every test case in every collection, and disposed of after every case.
/// </summary>
/// <param name="testContextAccessor">The context accessor provided by xUnit</param>
/// <param name="fixture"></param>
public abstract class IntegrationTestBase(ITestContextAccessor testContextAccessor, IntegrationTestFixture fixture) : IAsyncLifetime
{
    public IntegrationTestContextProvider TestContext = null!;
    protected CancellationToken CancellationToken => testContextAccessor.Current.CancellationToken;

    /// <summary>
    /// Called before every test case.
    /// </summary>
    public async ValueTask InitializeAsync()
    {
        // Each test case gets it's own integration test context
        var defaultTestContext = await fixture.CreateTestContext();
        TestContext = new IntegrationTestContextProvider
        {
            Default = defaultTestContext,
            CreateBuilder = fixture.TestContextBuilder
        };
    }

    /// <summary>
    /// Called after every test case.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await TestContext.Default.DisposeAsync();
    }

    protected async Task AdminSignIn()
        => await AdminSignIn(TestContext.Default);
    protected async Task AdminSignIn(IntegrationTestContext context)
        => await SignIn(context, TestUsers.GameAdminUserName, TestUsers.GameAdminUserPassword);

    protected async Task UserSignIn() => await UserSignIn(TestContext.Default);
    protected async Task UserSignIn(IntegrationTestContext context)
        => await SignIn(context, TestUsers.AppUserName, TestUsers.AppUserPassword);

    protected async Task SignIn(string username, string password)
        => await SignIn(TestContext.Default, username, password);
    protected async Task SignIn(IntegrationTestContext context, string username, string password)
    {
        var requestBody = new SignInRequestBody
        {
            Username = username,
            Password = password
        };

        var response = await context.Client.PostAsJsonAsync(
            "/account/signIn",
            requestBody,
            CancellationToken);

        response.EnsureSuccessStatusCode();
    }

    protected async Task SignOut()
        => await SignOut(TestContext.Default);
    protected async Task SignOut(IntegrationTestContext context)
    {
        var response = await context.Client.PostAsync(
            "/account/signOut",
            null,
            CancellationToken);

        response.EnsureSuccessStatusCode();
    }

    protected async Task<CreateGameResponse> CreateGame(CreateGameRequestBody? request = null)
        => await CreateGame(TestContext.Default, request);
    protected async Task<CreateGameResponse> CreateGame(IntegrationTestContext context, CreateGameRequestBody? request = null)
    {
        request ??= new CreateGameRequestBody
        {
            StartTime = context.TimeProvider.UtcNow.AddMinutes(5).UtcDateTime,
            CloseTime = context.TimeProvider.UtcNow.AddMinutes(10).UtcDateTime,
            DrawTime = context.TimeProvider.UtcNow.AddMinutes(15).UtcDateTime,
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

        var response = await context.Client.PostAsJsonAsync(
            "/game",
            request,
            CancellationToken);

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<Result<CreateGameResponse>>(CancellationToken);

        Assert.NotNull(body);
        Assert.NotNull(body.Value);
        Assert.Null(body.Errors);
        Assert.Equal(ResultStatus.Ok, body.Status);
        return body.Value;
    }

    protected async Task<IEnumerable<SearchGamesResponseItem>> SearchGames(SearchGamesRequestQuery? request = null, int? expectedTotalGames = 1)
        => await SearchGames(TestContext.Default, request, expectedTotalGames);
    protected async Task<IEnumerable<SearchGamesResponseItem>> SearchGames(IntegrationTestContext context, SearchGamesRequestQuery? request = null, int? expectedTotalGames = 1)
    {
        request ??= new SearchGamesRequestQuery
        {
            GameStatus = [GameStatus.Open, GameStatus.Future],
            Limit = 10,
            Page = 1,
            Name = "Test",
            SortBy = SearchGamesRequestQuery.SortCriteria.CloseTime,
            SortDirection = Api.Models.Common.SortDirection.Asc
        };

        var queryString = BuildQueryString(request);
        var response = await context.Client.GetAsync(
            $"/game/search?{queryString}",
            CancellationToken);

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
        Assert.Equal(request.Limit, body.Value.Limit);
        Assert.Equal(request.Page, body.Value.Page);
        Assert.Equal(expectedTotalGames, body.Value.Total);
        return body.Value.Items;
    }

    protected async Task<GetGameResponse> GetGame(Guid gameId) => await GetGame(TestContext.Default, gameId);
    protected async Task<GetGameResponse> GetGame(IntegrationTestContext context, Guid gameId)
    {
        var response = await context.Client.GetFromJsonAsync<Result<GetGameResponse>>(
            $"/game/{gameId}",
            CancellationToken);

        Assert.NotNull(response);
        Assert.Equal(ResultStatus.Ok, response.Status);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Value);
        return response.Value;
    }

    protected async Task<CreateEntryResponse> CreateEntry(Guid gameId, IEnumerable<int>? selectionNumbers = null)
        => await CreateEntry(TestContext.Default, gameId, selectionNumbers);
    protected async Task<CreateEntryResponse> CreateEntry(IntegrationTestContext context, Guid gameId, IEnumerable<int>? selectionNumbers = null)
    {
        selectionNumbers ??= [11, 12, 13, 14, 15];

        var requestBody = new CreateEntryRequestBody
        {
            GameId = gameId,
            Selections = selectionNumbers.Select(CreateEntryRequestBody.Selection.Create).ToList()
        };

        var response = await context.Client.PostAsJsonAsync(
            "/entry",
            requestBody,
            CancellationToken);

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<Result<CreateEntryResponse>>();
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.Ok, body.Status);
        Assert.Null(body.Errors);
        Assert.NotNull(body.Value);
        return body.Value;
    }

    protected async Task EditEntry(Guid entryId, IEnumerable<int>? selectionNumbers = null)
        => await EditEntry(TestContext.Default, entryId, selectionNumbers);
    protected async Task EditEntry(IntegrationTestContext context, Guid entryId, IEnumerable<int>? selectionNumbers = null)
    {
        selectionNumbers ??= [21, 22, 23, 24, 25];
        var requestBody = new EditEntryRequestBody
        {
            Selections = [.. selectionNumbers.Select(EditEntryRequestBody.Selection.Create)]
        };
        var response = await context.Client.PostAsJsonAsync(
            $"/entry/{entryId}/edit",
            requestBody,
            CancellationToken);

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<Result<CreateEntryResponse>>();
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.Ok, body.Status);
        Assert.Null(body.Errors);
        Assert.NotNull(body.Value);
    }

    protected async Task AdminResultGame(Guid gameId, IEnumerable<int>? winningSelectionNumbers = null, bool randomWinningSelectionNumbers = false)
        => await AdminResultGame(TestContext.Default, gameId, winningSelectionNumbers, randomWinningSelectionNumbers);
    protected async Task AdminResultGame(IntegrationTestContext context, Guid gameId, IEnumerable<int>? winningSelectionNumbers = null, bool randomWinningSelectionNumbers = false)
    {
        if (randomWinningSelectionNumbers)
        {
            winningSelectionNumbers = [];
        }
        else if (winningSelectionNumbers is null || winningSelectionNumbers.Count() <= 0)
        {
            winningSelectionNumbers ??= [21, 32, 33, 34, 35]; // 1 matching number, lowest prize   
        }

        var request = new ResultGameRequestBody
        {
            WinningSelections = [.. winningSelectionNumbers.Select(ResultGameRequestBody.GameSelection.Create)]

        };
        var response = await context.Client.PostAsync(
            $"/game/{gameId}/result",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<Result<ResultGameResponse>>();

        Assert.NotNull(body);
        Assert.Equal(ResultStatus.Ok, body.Status);
        Assert.Null(body.Errors);
        Assert.NotNull(body.Value);
    }

    protected async Task<IEnumerable<SearchEntriesResponseItem>> SearchEntries(Guid? gameId = null)
        => await SearchEntries(TestContext.Default, gameId);
    protected async Task<IEnumerable<SearchEntriesResponseItem>> SearchEntries(IntegrationTestContext context, Guid? gameId = null)
    {
        var request = new SearchEntriesRequestQuery
        {
            GameId = gameId,
        };

        var response = await context.Client.GetFromJsonAsync<Result<SearchEntriesResponse>>(
            $"/entry?{BuildQueryString(request)}",
            CancellationToken);

        Assert.NotNull(response);
        Assert.Equal(ResultStatus.Ok, response.Status);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Value);
        return response.Value.Items;
    }

    protected async Task TryUserResultGame_ExpectForbidden(Guid gameId) => await TryUserResultGame_ExpectForbidden(TestContext.Default, gameId);
    protected async Task TryUserResultGame_ExpectForbidden(IntegrationTestContext context, Guid gameId)
    {
        var request = new ResultGameRequestBody
        {
            WinningSelections = []
        };

        var response = await context.Client.PostAsync(
            $"/game/{gameId}/result",
            JsonContent.Create(request),
            CancellationToken);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    protected async Task TryEnterClosedGame_ExpectBadRequest(Guid gameId)
        => await TryEnterClosedGame_ExpectBadRequest(TestContext.Default, gameId);
    protected async Task TryEnterClosedGame_ExpectBadRequest(IntegrationTestContext context, Guid gameId)
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

        var response = await context.Client.PostAsJsonAsync(
            "/entry",
            requestBody,
            CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<Result<CreateEntryResponse>>();
        Assert.NotNull(body);
        Assert.Equal(ResultStatus.BadRequest, body.Status);
        Assert.NotNull(body.Errors);
        var error = Assert.Single(body.Errors);
        Assert.Equal($"Game {gameId} is closed", error.Message);
        Assert.Null(body.Value);
    }

    protected string BuildQueryString(object obj)
    {
        var values = new Dictionary<string, string?>();

        foreach (var property in obj.GetType().GetProperties())
        {
            var value = property.GetValue(obj);

            if (value is null)
            {
                continue;
            }

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