
namespace Lottery.Api.Models.Entry.Search;

public class SearchEntriesResponseItem
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public List<Selection> Selections { get; set; } = [];
    public EntryPrize? Prize { get; set; } = null;

    public class Selection
    {
        public Guid Id { get; set; }
        public int SelectionNumber { get; set; }
    }

    public class EntryPrize
    {
        public Guid Id { get; set; }
        public int Position { get; set; }
        public int NumberMatchCount { get; set; }
    }
}