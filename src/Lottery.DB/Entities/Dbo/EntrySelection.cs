using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Entities.Base;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Entities.Dbo;

[Table(nameof(EntrySelection), Schema = nameof(Dbo))]
// A player can only select a single selection once per entry
[Index(nameof(EntryId), nameof(GameSelectionId), IsUnique = true)]
/// <summary>
/// Class representing a number that is included on a players entry.
/// </summary>
public class EntrySelection : EntityObject
{
    /// <summary>
    /// The primary key of the <see cref="Dbo.Entry"/> that this selection belongs to.
    /// </summary>
    [Required]
    public required Guid EntryId { get; set; }

    /// <summary>
    /// The primary key of the <see cref="Dbo.GameSelection"/> that this selection belongs to.
    /// </summary>
    /// <value></value>
    [Required]
    public Guid GameSelectionId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The entry that this selection belongs to. 
    /// 
    /// Although this will always exist, it may be null
    /// if the query fetching the entry selection does not include it.
    /// </summary>
    public Entry Entry { get; set; } = default!;

    /// <summary>
    /// The game selection that this selection belongs points to. 
    /// 
    /// Although this will always exist, it may be null
    /// if the query fetching the entry selection does not include it.
    /// </summary>
    public GameSelection GameSelection { get; set; } = default!;
    #endregion

}