using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Entities.Base;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Entities.Dbo;

[Table(nameof(GameResult), Schema = nameof(Dbo))]
// A number can only be drawn once per game.
[Index(nameof(GameId), nameof(SelectionId), IsUnique = true)]
/// <summary>
/// Entity representing a single number that was drawn when a game gets resulted.
/// </summary>
public class GameResult : EntityObject
{
    /// <summary>
    /// The primary key of the game that this result relates to.
    /// </summary>
    [Required]
    public required Guid GameId { get; set; }

    /// <summary>
    /// The primary key of the game selection that this result relates to. 
    /// 
    /// This is how we determine which number this result represents.
    /// </summary>
    [Required]
    public required Guid SelectionId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The game that this result relates to.
    /// 
    /// Although this will always exist, it may be null if the query
    /// fetching the result does not include it.
    /// </summary>
    public Game Game { get; set; } = default!;

    /// <summary>
    /// The selection that this result relates to.
    /// 
    /// This is how we determine which number this result represents.
    /// 
    /// Although this will always exist, it may be null if the query
    /// fetching the result does not include it.
    /// </summary>
    public GameSelection Selection { get; set; } = default!;
    #endregion
}