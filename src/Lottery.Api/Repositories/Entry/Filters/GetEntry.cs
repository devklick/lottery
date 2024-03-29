using Lottery.DB.Entities.Ref;

namespace Lottery.Api.Repositories.Entry.Filters;

public class GetEntry
{
    public class GameFilter
    {
        public bool Include { get; set; }
        public GameSelectionFilter SelectionFilter { get; set; } = new GameSelectionFilter();

        public class GameSelectionFilter
        {
            public bool Include { get; set; }
            public ItemState? State { get; set; }
        }
    }

    public class SelectionsFilter
    {
        public bool Include { get; set; }
        public ItemState? State { get; set; }
    }
}