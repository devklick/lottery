
using System.Net.Quic;

using Lottery.DB.Context;
using Lottery.DB.Repository;
using Lottery.DB.Repository.Common;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Lottery.Api.Repositories.Game;

using GameEntity = DB.Entities.Dbo.Game;

public class GameRepository(LotteryDBContext db) : RepositoryBase<LotteryDBContext>(db)
{
    public async Task<GameEntity> CreateGame(GameEntity game)
    {
        var result = await _db.Games.AddAsync(game);

        await _db.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<GameEntity?> GetGame(Guid gameId)
        => await _db.Games
            .Include(g => g.Selections)
            .Include(g => g.Prizes)
            .Include(g => g.Results)
            .FirstOrDefaultAsync(g => g.Id == gameId);

    public async Task<(IEnumerable<GameEntity> Games, int Total)> SearchGames(int page, int limit, string? name = null, List<SearchGamesInState>? states = default, SearchGamesSortCriteria sortBy = SearchGamesSortCriteria.DrawTime, SortDirection sortDirection = SortDirection.Asc)
    {
        states ??= [SearchGamesInState.CanEnter];

        var query = _db.Games
            .Include(x => x.Selections)
            .Include(x => x.Prizes)
            .Include(x => x.Results)
            .AsQueryable();

        // TODO: This wont use the index. Best looking into collation
        if (name != null)
        {
            query = query.Where(g => EF.Functions.ILike(g.Name, $"%{name}%"));
        }

        // TODO: Figure a better, more extendable way of building this criteria

        if (states.Contains(SearchGamesInState.CanEnter) && states.Contains(SearchGamesInState.Future) && states.Contains(SearchGamesInState.CanEnter))
        {
            // all to be included, so no filters to apply here
        }
        else if (states.Contains(SearchGamesInState.CanEnter) && states.Contains(SearchGamesInState.Future))
        {
            query = query.Where(game => (game.StartTime <= DateTime.UtcNow && game.DrawTime > DateTime.UtcNow)
            || (game.StartTime >= DateTime.UtcNow));
        }
        else if (states.Contains(SearchGamesInState.CanEnter) && states.Contains(SearchGamesInState.Resulted))
        {
            query = query.Where(game => (game.StartTime <= DateTime.UtcNow && game.DrawTime > DateTime.UtcNow)
            || (game.ResultedAt != null && game.ResultedAt <= DateTime.UtcNow));
        }
        else if (states.Contains(SearchGamesInState.Future) && states.Contains(SearchGamesInState.Resulted))
        {
            query = query.Where(game => (game.StartTime >= DateTime.UtcNow)
            || (game.ResultedAt != null && game.ResultedAt <= DateTime.UtcNow));
        }
        else if (states.Contains(SearchGamesInState.CanEnter))
        {
            query = query.Where(game => game.StartTime <= DateTime.UtcNow && game.DrawTime >= DateTime.UtcNow);
        }
        else if (states.Contains(SearchGamesInState.Future))
        {
            query = query.Where(game => game.StartTime > DateTime.UtcNow);
        }
        else if (states.Contains(SearchGamesInState.Resulted))
        {
            query = query.Where(game => game.ResultedAt != null && game.ResultedAt <= DateTime.UtcNow);
        }

        switch (sortBy)
        {
            case SearchGamesSortCriteria.DrawTime:
                query = sortDirection == SortDirection.Asc ? query.OrderBy(g => g.DrawTime) : query.OrderByDescending(g => g.DrawTime);
                break;
            case SearchGamesSortCriteria.StartTime:
                query = sortDirection == SortDirection.Asc ? query.OrderBy(g => g.StartTime) : query.OrderByDescending(g => g.StartTime);
                break;
        }

        var total = await query.CountAsync();

        var games = await query
            .Skip((page - 1) * limit)
            .Take(limit).ToListAsync();

        return (games, total);
    }
}