using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Lottery.DB.Entities.Base;

namespace Lottery.DB.Entities.Dbo;

// TODO; Consider indexing on dates to allow searching for active games

[Table(nameof(Game), Schema = nameof(Dbo))]
/// <summary>
/// An entity representing a scheduled game which players can enter into.
/// </summary>
public class Game : EntityObject
{
    /// <summary>
    /// The date and time that the game starts. This is the earliest possible 
    /// time that players can submit their entries into the game.
    /// </summary>
    [Required]
    public required DateTime StartTime { get; set; }

    /// <summary>
    /// The date and time that entries will no longer be accepted. 
    /// This date is expected to be on or shortly before the <see cref="DrawTime"/>.
    /// </summary>
    [Required]
    public required DateTime CloseTime { get; set; }

    /// <summary>
    /// The date and time that the results will be drawn. 
    /// Players can enter the game up until this time.
    /// </summary>
    [Required]
    public required DateTime DrawTime { get; set; }

    /// <summary>
    /// The date and time at which the game was resulted.
    /// If the game has not yet been resulted, this will be null.
    /// </summary>
    public DateTime? ResultedAt { get; set; }

    /// <summary>
    /// The name allocated to the game.
    /// </summary>
    [Required, StringLength(64)]
    public required string Name { get; set; }

    /// <summary>
    /// The count of numbers that players have to select
    /// when submitting their entry to the game.
    /// 
    /// This is the same count of numbers that are draw
    /// when the game is resulted.
    /// </summary>
    [Required]
    public required int SelectionsRequiredForEntry { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The selections linked to the game. 
    /// 
    /// These are the possible selections that players can make
    /// when submitting their entry.
    /// </summary>
    public List<GameSelection> Selections { get; set; } = [];

    /// <summary>
    /// The possible prizes that can be won in this game.
    /// </summary>
    public List<GamePrize> Prizes { get; set; } = [];

    /// <summary>
    /// The entries that were submitted against this game.
    /// </summary>
    public List<Entry> Entries { get; set; } = [];

    public List<GameResult> Results { get; set; } = [];

    #endregion

    #region Helpers
    /// <summary>
    /// Helper to indicate what state the game is in
    /// </summary>
    public GameStatus GameStatus
    {
        get
        {
            if (StartTime > DateTime.UtcNow) return GameStatus.Future;
            if (DrawTime > DateTime.UtcNow) return GameStatus.Open;
            if (!ResultedAt.HasValue) return GameStatus.Closed;
            return GameStatus.Resulted;
        }
    }
    #endregion
}

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