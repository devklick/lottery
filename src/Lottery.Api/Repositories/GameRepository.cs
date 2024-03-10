
using Lottery.DB.Context;
using Lottery.DB.Entities.Dbo;
using Lottery.DB.Entities.Ref;
using Lottery.DB.Repository;

using Microsoft.EntityFrameworkCore;

namespace Lottery.Api.Repositories;

public class GameRepository(LotteryDBContext db) : RepositoryBase<LotteryDBContext>(db)
{
    public async Task<Game> CreateGame(Game game)
    {
        var result = await _db.Games.AddAsync(game);

        await _db.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<List<Game>> GetActiveGames()
    {
        var now = DateTime.UtcNow;
        return await _db.Games.Where(g =>
            g.State == ItemState.Enabled
            && g.StartTime <= now
            && g.DrawTime <= now
        )
        .Include(g => g.Selections)
        .Include(g => g.Prizes)
        .ToListAsync();
    }

    public async Task<Game?> GetGame(Guid gameId)
        => await _db.Games.Include(g => g.Selections).FirstOrDefaultAsync(g => g.Id == gameId);
}