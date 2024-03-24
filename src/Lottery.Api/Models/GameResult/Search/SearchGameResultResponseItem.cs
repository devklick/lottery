namespace Lottery.Api.Models.GameResult.Search;

public class SearchGameResultResponseItem
{
    public Guid Id { get; set; }
    public Guid SelectionId { get; set; }
    public int SelectionNumber { get; set; }
}