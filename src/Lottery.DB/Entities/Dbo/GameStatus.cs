using System.Text.Json.Serialization;

namespace Lottery.DB.Entities.Dbo;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GameStatus
{
    /// <summary>
    /// Not yet open, meaning unable to accept entries
    /// </summary>
    Future,
    /// <summary>
    /// Open and able to accept entries
    /// </summary>
    Open,
    /// <summary>
    /// No longer able to accept entries, but results not yet drawn
    /// </summary>
    Closed,
    /// <summary>
    /// Closed and results drawn. Will not move from this state.
    /// </summary>
    Resulted
}