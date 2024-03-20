using Lottery.Api.Models.Common;

namespace Lottery.Api.Models.Game.Search;

public class SearchGamesRequestQuery : PagedRequest
{
    public string? Name { get; set; }
    public List<SearchGameState> GameStates { get; set; } = [SearchGameState.CanEnter];
    public SearchGamesSortCriteria SortBy { get; set; } = SearchGamesSortCriteria.DrawTime;
    public SortDirection SortDirection { get; set; } = SortDirection.Asc;
}