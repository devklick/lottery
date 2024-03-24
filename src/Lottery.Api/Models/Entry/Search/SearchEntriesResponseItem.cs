using Lottery.Api.Models.EntrySelection.Search;
using Lottery.Api.Models.EntryPrize.Search;

namespace Lottery.Api.Models.Entry.Search;

public class SearchEntriesResponseItem
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public List<SearchEntrySelectionResponseItem> Selections { get; set; } = [];
    public SearchEntryPrizeResponseItem? Prize { get; set; } = null;
}