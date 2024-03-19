
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

    public async Task<Game?> GetGame(Guid gameId)
        => await _db.Games.Include(g => g.Selections).FirstOrDefaultAsync(g => g.Id == gameId);

    public async Task<(IEnumerable<Game> Games, int Total)> SearchGames(int page, int limit)
    {
        var query = _db.Games
            .Include(x => x.Selections)
            .Include(x => x.Prizes)
            .OrderByDescending(e => e.CreatedOnUtc);

        var total = await query.CountAsync();
        var games = await query.Skip((page - 1) * limit)
            .Take(limit).ToListAsync();

        return (games, total);
    }
}