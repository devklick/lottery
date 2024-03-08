using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Attributes;
using Lottery.DB.Entities.Base;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Entities.Dbo;

[Table(nameof(GamePrize), Schema = nameof(Dbo))]
// Only prizes with one combination of game + position can be allowed. 
// Have a game with multiple prizes using the same position will not result properly.
[Index(nameof(GameId), nameof(Position), IsUnique = true)]
// Only prizes with one combination of game + number match count can be allowed. 
// Have a game with multiple prizes using the same position will not result properly.
[Index(nameof(GameId), nameof(NumberMatchCount), IsUnique = true)]
/// <summary>
/// An entity representing a prize that can be won during a game
/// based on whether or not any players predict the correct numbers.
/// </summary>
public class GamePrize : EntityObject
{
    /// <summary>
    /// The primary key of the game that this prize relates to.
    /// </summary>
    [Required]
    public required Guid GameId { get; set; }

    /// <summary>
    /// The position represents the tiered prizes, where the top prize is number 1, 
    /// and the lower prizes are higher numbers.
    /// </summary>
    [Required]
    public required int Position { get; set; }

    /// <summary>
    /// The number of numbers that the player must have correctly predicted. 
    /// </summary>
    /// <value></value>
    [Required]
    public required int NumberMatchCount { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The game that the prize relates to.
    /// 
    /// Although this will always exist, it may be null if not included
    /// in the query that's fetching the prize.
    /// </summary>
    public required Game Game { get; set; }

    /// <summary>
    /// The entries that won this prize, if any.
    /// </summary>
    public List<EntryPrize> EntryPrizes { get; set; } = [];
    #endregion
}