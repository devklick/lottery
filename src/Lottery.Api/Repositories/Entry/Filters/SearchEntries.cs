using Lottery.DB.Entities.Ref;

namespace Lottery.Api.Repositories.Entry.Filters;

public class SearchEntries
{
    public class EntryFilter
    {
        public Guid? UserId { get; set; }
        public ItemState? State { get; set; }
    }
    public class GameFilter
    {
        public Guid? GameId { get; set; }
        public bool Include { get; set; }
        public ItemState? State { get; set; }
    }
    public class SelectionsFilter
    {
        public bool Include { get; set; }
        public ItemState? State { get; set; }
    }

    public class PrizeFilter
    {
        public bool Include { get; set; }
        public ItemState? State { get; set; }
    }
}