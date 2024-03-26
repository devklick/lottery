
using Lottery.Api.Repositories.Game.Filters;
using Lottery.DB.Context;
using Lottery.DB.Entities.Dbo;
using Lottery.DB.Repositories;
using Lottery.DB.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace Lottery.Api.Repositories.Game;

using GameEntity = DB.Entities.Dbo.Game;



public partial class GameRepository(LotteryDBContext db) : RepositoryBase<LotteryDBContext>(db)
{
    public async Task<GameEntity> CreateGame(GameEntity game)
    {
        var result = await _db.Games.AddAsync(game);

        await _db.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<GameEntity?> GetGame(
        Guid gameId,
        GetGame.SelectionFilter? selectionsFilter = null,
        GetGame.PrizesFilter? prizesFilter = null,
        GetGame.ResultsFilter? resultsFilter = null)
    {
        var query = _db.Games.Where(g => g.Id == gameId).AsQueryable();

        if ((selectionsFilter?.Include) ?? false)
        {
            query = query.Include(g => g.Selections);
            if (selectionsFilter.State.HasValue)
            {
                query = query.Where(g => g.Selections.Any(s => s.State == selectionsFilter.State));
            }
        }

        if ((prizesFilter?.Include) ?? false)
        {
            query = query.Include(g => g.Prizes);
            if (prizesFilter.State.HasValue)
            {
                query = query.Where(g => g.Selections.Any(s => s.State == prizesFilter.State));
            }
        }

        if ((resultsFilter?.Include) ?? false)
        {
            query = query.Include(g => g.Results);
            if (resultsFilter.State.HasValue)
            {
                query = query.Where(g => g.Selections.Any(s => s.State == resultsFilter.State));
            }
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<GameEntity> Games, int Total)> SearchGames(
        int page, int limit, string? name = null,
        List<GameStatus>? states = default,
        SearchGames.SortCriteria sortBy = Filters.SearchGames.SortCriteria.DrawTime,
        SortDirection sortDirection = SortDirection.Asc)
    {
        states ??= [GameStatus.Open];

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

        if (states.Contains(GameStatus.Open) && states.Contains(GameStatus.Future) && states.Contains(GameStatus.Resulted))
        {
            // all to be included, so no filters to apply here
        }
        else if (states.Contains(GameStatus.Open) && states.Contains(GameStatus.Future))
        {
            query = query.Where(game => (game.StartTime <= DateTime.UtcNow && game.DrawTime > DateTime.UtcNow)
            || (game.StartTime >= DateTime.UtcNow));
        }
        else if (states.Contains(GameStatus.Open) && states.Contains(GameStatus.Resulted))
        {
            query = query.Where(game => (game.StartTime <= DateTime.UtcNow && game.DrawTime > DateTime.UtcNow)
            || (game.ResultedAt != null && game.ResultedAt <= DateTime.UtcNow));
        }
        else if (states.Contains(GameStatus.Future) && states.Contains(GameStatus.Resulted))
        {
            query = query.Where(game => (game.StartTime >= DateTime.UtcNow)
            || (game.ResultedAt != null && game.ResultedAt <= DateTime.UtcNow));
        }
        else if (states.Contains(GameStatus.Open))
        {
            query = query.Where(game => game.StartTime <= DateTime.UtcNow && game.DrawTime >= DateTime.UtcNow);
        }
        else if (states.Contains(GameStatus.Future))
        {
            query = query.Where(game => game.StartTime > DateTime.UtcNow);
        }
        else if (states.Contains(GameStatus.Resulted))
        {
            query = query.Where(game => game.ResultedAt != null && game.ResultedAt <= DateTime.UtcNow);
        }

        switch (sortBy)
        {
            case Filters.SearchGames.SortCriteria.DrawTime:
                query = sortDirection == SortDirection.Asc ? query.OrderBy(g => g.DrawTime) : query.OrderByDescending(g => g.DrawTime);
                break;
            case Filters.SearchGames.SortCriteria.StartTime:
                query = sortDirection == SortDirection.Asc ? query.OrderBy(g => g.StartTime) : query.OrderByDescending(g => g.StartTime);
                break;
            case Filters.SearchGames.SortCriteria.CloseTime:
                query = sortDirection == SortDirection.Asc ? query.OrderBy(g => g.CloseTime) : query.OrderByDescending(g => g.CloseTime);
                break;
        }

        var total = await query.CountAsync();

        var games = await query
            .Skip((page - 1) * limit)
            .Take(limit).ToListAsync();

        return (games, total);
    }

    internal async Task<GameEntity> UpdateGame(GameEntity game)
    {
        _db.Games.Update(game);

        await _db.SaveChangesAsync();

        return game;
    }
}