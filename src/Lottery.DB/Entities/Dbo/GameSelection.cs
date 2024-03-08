using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Entities.Base;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Entities.Dbo;

[Table(nameof(GameSelection), Schema = nameof(Dbo))]
// A number can only exist once per game.
[Index(nameof(GameId), nameof(SelectionNumber), IsUnique = true)]
/// <summary>
/// An entity representing a number that can be picked by players.
/// </summary>
public class GameSelection : EntityObject
{
    /// <summary>
    /// The primary key of the game that this selection relates to.
    /// </summary>
    [Required]
    public required Guid GameId { get; set; }

    /// <summary>
    /// The number that players can pick.
    /// </summary>
    [Required]
    public required int SelectionNumber { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The game that this selection relates to. 
    /// 
    /// Although this will always exist, it may be null if 
    /// the query returning the selection does not include it.
    /// </summary>
    public Game Game { get; set; } = default!;
    #endregion
}