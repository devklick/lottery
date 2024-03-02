using Lottery.Repository.Context;
using Lottery.Repository.Entities.Dbo;

namespace Lottery.Repository;

public class GameRepository(LotteryDBContext db)
{
    private readonly LotteryDBContext _db = db;

    public async Task<Game> CreateGame(Game game)
    {
        var result = await _db.Games.AddAsync(game);

        await _db.SaveChangesAsync();

        return result.Entity;
    }
}