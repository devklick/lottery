using System.Linq.Expressions;

using Lottery.Common.Comparers;

using Lottery.Common.Extensions;

using Lottery.DB.Entities.Dbo;
using Lottery.DB.Repositories.Common;

using GameEntity = Lottery.DB.Entities.Dbo.Game;

namespace Lottery.Api.Repositories.Game;
public static class GameRepositoryExtensions
{
    // For each game status, we'll hold an expression that checks 
    // if a game falls into this status based on various criteria
    private static readonly Expression<Func<GameEntity, bool>> FutureFilter = game =>
        game.StartTime > DateTime.UtcNow && !game.ResultedAt.HasValue;

    private static readonly Expression<Func<GameEntity, bool>> ClosedFilter = game =>
        game.CloseTime <= DateTime.UtcNow && !game.ResultedAt.HasValue;

    private static readonly Expression<Func<GameEntity, bool>> OpenFilter = game =>
        game.StartTime <= DateTime.UtcNow && game.DrawTime >= DateTime.UtcNow && !game.ResultedAt.HasValue;

    private static readonly Expression<Func<GameEntity, bool>> ResultedFilter = game =>
        game.ResultedAt.HasValue;

    // Map the different filters by their corresponding game status
    private static readonly Dictionary<GameStatus, Expression<Func<GameEntity, bool>>> StatusToFilterMap = new()
    {
        { GameStatus.Closed, ClosedFilter},
        { GameStatus.Future, FutureFilter},
        { GameStatus.Open, OpenFilter},
        { GameStatus.Resulted, ResultedFilter}
    };

    // Generate a dictionary where each item has:
    // - key: one of the possible combinations of game statuses
    // - value: the corresponding expressions for each status in the key, combined using OR criteria.
    private static readonly Dictionary<IEnumerable<GameStatus>, Expression<Func<GameEntity, bool>>> StatusesToFilterMap = new(
        Enum.GetValues<GameStatus>()
            .GetPermutations()
            .ToDictionary(key => key, val => val.Aggregate(
                default(Expression<Func<GameEntity, bool>>)!,
                (res, cur) => res == null ? StatusToFilterMap[cur] : res.OrElse(StatusToFilterMap[cur]))),
        new UnorderedListComparer<GameStatus>());

    public static IQueryable<GameEntity> FilterByGameStatuses(this IQueryable<GameEntity> query, List<GameStatus> statuses)
        => query.Where(StatusesToFilterMap[statuses]);

    public static IQueryable<GameEntity> SortBy(this IQueryable<GameEntity> query, Filters.SearchGames.SortCriteria sortBy, SortDirection sortDirection)
    {
        return sortBy switch
        {
            Filters.SearchGames.SortCriteria.DrawTime => query = sortDirection == SortDirection.Asc ? query.OrderBy(g => g.DrawTime) : query.OrderByDescending(g => g.DrawTime),
            Filters.SearchGames.SortCriteria.StartTime => query = sortDirection == SortDirection.Asc ? query.OrderBy(g => g.StartTime) : query.OrderByDescending(g => g.StartTime),
            Filters.SearchGames.SortCriteria.CloseTime => query = sortDirection == SortDirection.Asc ? query.OrderBy(g => g.CloseTime) : query.OrderByDescending(g => g.CloseTime),
            _ => query,
        };

    }
}