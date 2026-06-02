namespace Lottery.Integration.Test.E2E;

[Collection("Integration")]
public class EndToEndTest(ITestContextAccessor testContextAccessor, IntegrationTestFixture fixture) : IntegrationTestBase(testContextAccessor, fixture)
{
    [Fact]
    public async Task SingleGame_SingleEntry_WinLowestPrize_OK()
    {
        // Admin signs in and creates a game that starts in the future
        await AdminSignIn();
        await CreateGame();

        // User signs in and searches for games, finds a game and opens it
        await SignOut();
        await UserSignIn();
        var foundGames = await SearchGames();
        var foundGame = Assert.Single(foundGames);
        var gameId = foundGame.Id;
        var game = await GetGame(gameId);

        // Time progresses so that the game is now open for entry, 
        // user creates an entry into the game
        TestContext.SetCurrentTime(game.StartTime);
        var createdEntry = await CreateEntry(gameId, [11, 12, 13, 14, 15]);
        var entryId = createdEntry.Id;

        // user edits their entry
        await EditEntry(entryId, [21, 22, 23, 24, 25]);

        // Time progresses and the game is now closed
        TestContext.SetCurrentTime(game.CloseTime);
        // User cannot submit an entry on a closed game
        await TryEnterClosedGame_ExpectBadRequest(gameId);

        // Time progresses and the game is now ready for resulting
        TestContext.SetCurrentTime(game.DrawTime);
        // User cannot result a game
        await TryUserResultGame_ExpectForbidden(gameId);

        // Admin signs in and results the game
        await SignOut();
        await AdminSignIn();
        await AdminResultGame(gameId, [21, 32, 33, 34, 35]); // 1 matching number, lowest prize   

        // User signs in and see's they have won a prize
        await SignOut();
        await UserSignIn();
        var entries = await SearchEntries(gameId);
        var entry = Assert.Single(entries);
        Assert.NotNull(entry.Prize);
        Assert.Equal(1, entry.Prize.NumberMatchCount);
        Assert.Equal(5, entry.Prize.Position);
    }

    [Fact]
    public async Task SingleGame_MultipleEntries_SinglePrize_WinLowestPrize_OK()
    {
        // // Admin signs in and creates a game that starts in the future
        // await AdminSignIn();
        // await CreateGame();

        // // User signs in and searches for games, finds a game and opens it
        // await SignOut();
        // await UserSignIn();
        // var foundGames = await SearchGames();
        // var foundGame = Assert.Single(foundGames);
        // var gameId = foundGame.Id;
        // var game = await GetGame(gameId);

        // // Time progresses so that the game is now open for entry, 
        // // user creates an entry into the game
        // fixture.TimeProvider.UtcNow = game.StartTime;
        // var createdEntry = await CreateEntry(gameId, [11, 12, 13, 14, 15]);
        // var entryId = createdEntry.Id;

        // // user edits their entry
        // await EditEntry(entryId, [21, 22, 23, 24, 25]);

        // // Time progresses and the game is now closed
        // fixture.TimeProvider.UtcNow = game.CloseTime;
        // // User cannot submit an entry on a closed game
        // await TryEnterClosedGame_ExpectBadRequest(gameId);

        // // Time progresses and the game is now ready for resulting
        // fixture.TimeProvider.UtcNow = game.DrawTime;
        // // User cannot result a game
        // await TryUserResultGame_ExpectForbidden(gameId);

        // // Admin signs in and results the game
        // await SignOut();
        // await AdminSignIn();
        // await AdminResultGame(gameId, [21, 32, 33, 34, 35]); // 1 matching number, lowest prize   

        // // User signs in and see's they have won a prize
        // await SignOut();
        // await UserSignIn();
        // var entries = await SearchEntries(gameId);
        // var entry = Assert.Single(entries);
        // Assert.NotNull(entry.Prize);
        // Assert.Equal(1, entry.Prize.NumberMatchCount);
        // Assert.Equal(5, entry.Prize.Position);
    }
}