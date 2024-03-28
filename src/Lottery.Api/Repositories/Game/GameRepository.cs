
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
        var query = _db.Games.Where(g => g.Id == gameId)
            .AsNoTracking();

        if ((selectionsFilter?.Include) ?? false)
        {
            query = query.Include(x => x.Selections.Where(s =>
                !selectionsFilter.State.HasValue || s.State == selectionsFilter.State));
        }

        if ((prizesFilter?.Include) ?? false)
        {
            query = query.Include(x => x.Prizes.Where(s =>
                !prizesFilter.State.HasValue || s.State == prizesFilter.State));
        }

        if ((resultsFilter?.Include) ?? false)
        {
            query = query.Include(x => x.Results.Where(s =>
                !resultsFilter.State.HasValue || s.State == resultsFilter.State));
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<GameEntity> Games, int Total)> SearchGames(
        int page, int limit,
        SearchGames.GamesFilter? gamesFilter,
        SearchGames.SelectionsFilter? selectionsFilter,
        SearchGames.PrizesFilter? prizesFilter,
        SearchGames.ResultsFilter? resultsFilter)
    {
        gamesFilter ??= new SearchGames.GamesFilter();

        var query = _db.Games.AsQueryable();

        if ((selectionsFilter?.Include) ?? false)
        {
            query = query.Include(x => x.Selections.Where(s =>
                !selectionsFilter.State.HasValue || s.State == selectionsFilter.State));
        }
        if ((prizesFilter?.Include) ?? false)
        {
            query = query.Include(x => x.Prizes.Where(p =>
                !prizesFilter.State.HasValue || p.State == prizesFilter.State));
        }
        if ((resultsFilter?.Include) ?? false)
        {
            query = query.Include(x => x.Results.Where(r =>
                !resultsFilter.State.HasValue || r.State == resultsFilter.State));
        }

        // TODO: This wont use the index. Best looking into collation
        if (gamesFilter.Name != null)
        {
            query = query.Where(g => EF.Functions.ILike(g.Name, $"%{gamesFilter.Name}%"));
        }

        // TODO: Figure a better, more extendable way of building these filters
        var states = gamesFilter.GameStates;
        var sortBy = gamesFilter.SortBy.Column;
        var sortDirection = gamesFilter.SortBy.Direction;
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

    public async Task<GameEntity> UpdateGame(GameEntity game)
    {
        _db.Games.Update(game);

        await _db.SaveChangesAsync();

        return game;
    }
}