using System.Text.Json.Serialization;

using Lottery.Api.Models.Common;
using Lottery.DB.Entities.Dbo;

namespace Lottery.Api.Models.Game.Search;

public class SearchGamesRequestQuery : PagedRequest
{
    public string? Name { get; set; }
    public List<GameStatus> GameStates { get; set; } = [GameStatus.Open];
    public SortCriteria SortBy { get; set; } = SortCriteria.DrawTime;
    public SortDirection SortDirection { get; set; } = SortDirection.Asc;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortCriteria
    {
        DrawTime,
        StartTime,
        CloseTime
    }
}