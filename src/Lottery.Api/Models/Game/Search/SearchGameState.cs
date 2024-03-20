using System.Text.Json.Serialization;

namespace Lottery.Api.Models.Game.Search;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SearchGameState
{
    Future,
    CanEnter,
    Resulted
}