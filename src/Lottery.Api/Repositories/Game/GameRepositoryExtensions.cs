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
    private static Expression<Func<GameEntity, bool>> FutureFilter(DateTimeOffset utcNow) =>
        game => game.StartTime > utcNow && !game.ResultedAt.HasValue;

    private static Expression<Func<GameEntity, bool>> ClosedFilter(DateTimeOffset utcNow) =>
        game => game.CloseTime <= utcNow && !game.ResultedAt.HasValue;

    private static Expression<Func<GameEntity, bool>> OpenFilter(DateTimeOffset utcNow) =>
        game => game.StartTime <= utcNow && game.DrawTime >= utcNow && !game.ResultedAt.HasValue;

    private static readonly Expression<Func<GameEntity, bool>> ResultedFilter = game =>
        game.ResultedAt.HasValue;

    public static IQueryable<GameEntity> FilterByGameStatuses(
        this IQueryable<GameEntity> query,
        IEnumerable<GameStatus> statuses,
        DateTimeOffset utcNow)
    {
        Expression<Func<GameEntity, bool>>? filter = null;

        foreach (var status in statuses)
        {
            var current = status switch
            {
                GameStatus.Future => FutureFilter(utcNow),
                GameStatus.Open => OpenFilter(utcNow),
                GameStatus.Closed => ClosedFilter(utcNow),
                GameStatus.Resulted => ResultedFilter,
                _ => throw new ArgumentOutOfRangeException(nameof(statuses), $"Status {status} invalid")
            };

            filter = filter == null
                ? current
                : filter.OrElse(current);
        }

        return filter == null
            ? query
            : query.Where(filter);
    }

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