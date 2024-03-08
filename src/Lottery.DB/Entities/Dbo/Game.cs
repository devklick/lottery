using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    /// The date and time that the results will be drawn. 
    /// Players can enter the game up until this time.
    /// </summary>
    [Required]
    public required DateTime DrawTime { get; set; }

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
    public required int NumbersRequired { get; set; }

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
    #endregion
}