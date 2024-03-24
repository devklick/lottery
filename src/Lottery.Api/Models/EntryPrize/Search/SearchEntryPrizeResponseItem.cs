namespace Lottery.Api.Models.EntryPrize.Search;

public class SearchEntryPrizeResponseItem
{
    public Guid Id { get; set; }
    public int Position { get; set; }
    public int NumberMatchCount { get; set; }
}