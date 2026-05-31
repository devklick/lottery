
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using Lottery.Api.Repositories.Game.Filters;
using Lottery.DB.Context;
using Lottery.DB.Repositories;
using Lottery.DB.Repositories.Common;

using Microsoft.EntityFrameworkCore;

namespace Lottery.Api.Repositories.Game;

using GameEntity = DB.Entities.Dbo.Game;


public partial class GameRepository(LotteryDBContext db, TimeProvider timeProvider) : RepositoryBase<LotteryDBContext>(db)
{
    private readonly TimeProvider _timeProvider = timeProvider;

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
        var query = _db.Games.Where(g => g.Id == gameId);
        // .AsNoTracking(); 
        // added back in tracking to fix resulting, but this will resurface 
        // another issue that I had previously. Will need to re-address that.

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

        query = query
            .FilterByGameStatuses(gamesFilter.GameStatus, _timeProvider.GetUtcNow())
            .SortBy(gamesFilter.SortBy.Column, gamesFilter.SortBy.Direction);

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