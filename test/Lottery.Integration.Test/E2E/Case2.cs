namespace Lottery.Integration.Test.E2E;

[Collection("Integration")]
public class Case2(ITestContextAccessor testContextAccessor, IntegrationTestFixture fixture) : IntegrationTestBase(testContextAccessor, fixture)
{
    [Fact]
    public async Task SingleGame_MultiplePlayers_SingleEntryEach_OnePlayerWins_SinglePrize()
    {
        var game = await Do_AdminCreateGame();

        TestContext.Default.SetCurrentTime(game.StartTime);

        await User1SignIn();
        await CreateEntry(game.Id, [1, 2, 3, 4, 5]);
        await SignOut();

        await User2SignIn();
        await CreateEntry(game.Id, [6, 7, 8, 9, 10]);
        await SignOut();

        TestContext.Default.SetCurrentTime(game.DrawTime);

        await Do_AdminResultGame(game.Id, [1, 2, 3, 4, 5]);

        await User1SignIn();
        var user1FoundEntries = await SearchEntries(game.Id);
        var user1FoundEntry = Assert.Single(user1FoundEntries);
        Assert.NotNull(user1FoundEntry.Prize);
        Assert.Equal(5, user1FoundEntry.Prize.NumberMatchCount);
        Assert.Equal(1, user1FoundEntry.Prize.Position);

        await User2SignIn();
        var user2FoundEntries = await SearchEntries(game.Id);
        var user2FoundEntry = Assert.Single(user2FoundEntries);
        Assert.Null(user2FoundEntry.Prize);
    }

    [Fact]
    public async Task SingleGame_MultiplePlayers_SingleEntryEach_BothPlayersWin_TopPrize()
    {
        var game = await Do_AdminCreateGame();

        TestContext.Default.SetCurrentTime(game.StartTime);

        await User1SignIn();
        await CreateEntry(game.Id, [1, 2, 3, 4, 5]);
        await SignOut();

        await User2SignIn();
        await CreateEntry(game.Id, [1, 2, 3, 4, 5]);
        await SignOut();

        TestContext.Default.SetCurrentTime(game.DrawTime);

        await Do_AdminResultGame(game.Id, [1, 2, 3, 4, 5]);

        await User1SignIn();
        var user1FoundEntries = await SearchEntries(game.Id);
        var user1FoundEntry = Assert.Single(user1FoundEntries);
        Assert.NotNull(user1FoundEntry.Prize);
        Assert.Equal(5, user1FoundEntry.Prize.NumberMatchCount);
        Assert.Equal(1, user1FoundEntry.Prize.Position);

        await User2SignIn();
        var user2FoundEntries = await SearchEntries(game.Id);
        var user2FoundEntry = Assert.Single(user2FoundEntries);
        Assert.NotNull(user2FoundEntry.Prize);
        Assert.Equal(5, user2FoundEntry.Prize.NumberMatchCount);
        Assert.Equal(1, user2FoundEntry.Prize.Position);
    }
}