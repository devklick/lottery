using Lottery.DB.Entities.Dbo;
using Lottery.DB.Entities.Ref;
using Lottery.DB.Repositories.Common;

namespace Lottery.Api.Repositories.Game.Filters;

public class SearchGames
{
    public enum SortCriteria
    {
        DrawTime,
        StartTime,
        CloseTime
    }

    public class GamesFilter
    {
        public string? Name { get; set; }
        public List<GameStatus> GameStates { get; set; } = [GameStatus.Open];
        public GamesSorting SortBy { get; set; } = new GamesSorting
        {
            Column = SortCriteria.DrawTime,
            Direction = SortDirection.Desc
        };
    }

    public class GamesSorting
    {
        public SortCriteria Column { get; set; }
        public SortDirection Direction { get; set; }
    }

    public class SelectionsFilter
    {
        public bool Include { get; set; }
        public ItemState? State { get; set; }
    }

    public class PrizesFilter
    {
        public bool Include { get; set; }
        public ItemState? State { get; set; }
    }

    public class ResultsFilter
    {
        public bool Include { get; set; }
        public ItemState? State { get; set; }
    }
}
