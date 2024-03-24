using Lottery.Api.Models.GameResult.Search;
using Lottery.Api.Models.GameSelection.Search;

namespace Lottery.Api.Models.Game.Search;

public class SearchGamesResponseItem
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime DrawTime { get; set; }
    public required int SelectionsRequiredForEntry { get; set; }
    public required List<SearchGameSelectionResponseItem> Selections { get; set; }
    public required List<SearchGameResultResponseItem> Results { get; set; }
    public required List<SearchGamePrizeResponseItem> Prizes { get; set; }
}