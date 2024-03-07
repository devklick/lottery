using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Entities.Base;

namespace Lottery.DB.Entities.Dbo;

[Table(nameof(GamePrize), Schema = nameof(Dbo))]
public class GamePrize : EntityObject
{
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

    public required Game Game { get; set; }

    /// <summary>
    /// The entries that won this prize
    /// </summary>
    public required List<EntryPrize> EntryPrizes { get; set; }
}