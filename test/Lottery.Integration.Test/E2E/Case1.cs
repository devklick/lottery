namespace Lottery.Integration.Test.E2E;

[Collection("Integration")]
public class Case1(ITestContextAccessor testContextAccessor, IntegrationTestFixture fixture) : IntegrationTestBase(testContextAccessor, fixture)
{
    [Fact]
    public async Task SingleGame_SinglePlayer_SingleEntry_WinLowestPrize_OK()
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
    public async Task SingleGame_SinglePlayer_MultipleEntries_SinglePrize_WinLowestPrize_OK()
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

        int[] winningNumbers = [1, 2, 3, 4, 5];

        // only 1 entries has a matching number
        var createdEntry1 = await CreateEntry(gameId, [1, 6, 7, 8, 9]);
        var createdEntry2 = await CreateEntry(gameId, [10, 11, 12, 13, 14]);

        // Time progresses and the game is now ready for resulting
        TestContext.SetCurrentTime(game.DrawTime);

        // Admin signs in and results the game
        await SignOut();
        await AdminSignIn();
        await AdminResultGame(gameId, winningNumbers); // 1 matching number, lowest prize   

        // User signs in and see's they have won a prize
        await SignOut();
        await UserSignIn();
        var entries = await SearchEntries(gameId);
        Assert.Equal(2, entries.Count());

        var entry1 = entries.First(e => e.Id == createdEntry1.Id);
        var entry2 = entries.First(e => e.Id == createdEntry2.Id);
        Assert.NotNull(entry1);
        Assert.NotNull(entry2);

        Assert.NotNull(entry1.Prize);
        Assert.Equal(1, entry1.Prize.NumberMatchCount);
        Assert.Equal(5, entry1.Prize.Position);
        Assert.Null(entry2.Prize);
    }

    [Fact]
    public async Task SingleGame_SinglePlayer_MultipleEntries_DifferentPrizePerEntry_OK()
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

        int[] winningNumbers = [1, 2, 3, 4, 5];

        var createdEntry1 = await CreateEntry(gameId, [1, 2, 3, 4, 5]); // 5 matching
        var createdEntry2 = await CreateEntry(gameId, [1, 2, 3, 4, 6]); // 4 matching
        var createdEntry3 = await CreateEntry(gameId, [1, 2, 3, 6, 7]); // 3 matching
        var createdEntry4 = await CreateEntry(gameId, [1, 2, 6, 7, 8]); // 2 matching
        var createdEntry5 = await CreateEntry(gameId, [1, 6, 7, 8, 9]); // 1 matching

        // Time progresses and the game is now ready for resulting
        TestContext.SetCurrentTime(game.DrawTime);

        // Admin signs in and results the game
        await SignOut();
        await AdminSignIn();
        await AdminResultGame(gameId, winningNumbers); // 1 matching number, lowest prize   

        // User signs in and see's they have won a prize
        await SignOut();
        await UserSignIn();
        var entries = await SearchEntries(gameId);
        Assert.Equal(5, entries.Count());
        var entry1 = entries.First(e => e.Id == createdEntry1.Id);
        var entry2 = entries.First(e => e.Id == createdEntry2.Id);
        var entry3 = entries.First(e => e.Id == createdEntry3.Id);
        var entry4 = entries.First(e => e.Id == createdEntry4.Id);
        var entry5 = entries.First(e => e.Id == createdEntry5.Id);

        Assert.NotNull(entry1);
        Assert.NotNull(entry2);
        Assert.NotNull(entry3);
        Assert.NotNull(entry4);
        Assert.NotNull(entry5);

        Assert.NotNull(entry1.Prize);
        Assert.Equal(5, entry1.Prize.NumberMatchCount);
        Assert.Equal(1, entry1.Prize.Position);

        Assert.NotNull(entry2.Prize);
        Assert.Equal(4, entry2.Prize.NumberMatchCount);
        Assert.Equal(2, entry2.Prize.Position);

        Assert.NotNull(entry3.Prize);
        Assert.Equal(3, entry3.Prize.NumberMatchCount);
        Assert.Equal(3, entry3.Prize.Position);

        Assert.NotNull(entry4.Prize);
        Assert.Equal(2, entry4.Prize.NumberMatchCount);
        Assert.Equal(4, entry4.Prize.Position);

        Assert.NotNull(entry5.Prize);
        Assert.Equal(1, entry5.Prize.NumberMatchCount);
        Assert.Equal(5, entry5.Prize.Position);
    }

    [Fact]
    public async Task SingleGame_SinglePlayer_MultipleEntries_SamePrizePerEntry_OK()
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

        int[] winningNumbers = [1, 2, 3, 4, 5];

        await Task.WhenAll(
            CreateEntry(gameId, [1, 2, 3, 10, 11]), // 3 matching
            CreateEntry(gameId, [1, 2, 3, 20, 21])); // 3 matching

        // Time progresses and the game is now ready for resulting
        TestContext.SetCurrentTime(game.DrawTime);

        // Admin signs in and results the game
        await SignOut();
        await AdminSignIn();
        await AdminResultGame(gameId, winningNumbers); // 1 matching number, lowest prize   

        // User signs in and see's they have won a prize
        await SignOut();
        await UserSignIn();
        var entries = await SearchEntries(gameId);
        Assert.Equal(2, entries.Count());
        foreach (var entry in entries)
        {
            Assert.NotNull(entry);

            Assert.NotNull(entry.Prize);
            Assert.Equal(3, entry.Prize.NumberMatchCount);
            Assert.Equal(3, entry.Prize.Position);
        }
    }
}