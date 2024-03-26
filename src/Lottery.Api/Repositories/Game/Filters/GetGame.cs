using Lottery.DB.Entities.Ref;

namespace Lottery.Api.Repositories.Game.Filters;

public class GetGame
{
    public class SelectionFilter
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