namespace Lottery.Api.Models.GameSelection.Search;

public class SearchGamePrizeResponseItem
{
    public required Guid Id { get; set; }
    public required int Position { get; set; }
    public required int NumberMatchCount { get; set; }
}