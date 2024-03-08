using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Attributes;
using Lottery.DB.Entities.Base;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Entities.Dbo;

[Table(nameof(EntryPrize), Schema = nameof(Dbo))]
// Only one entry prize should exist for a given game + entry
[Index(nameof(GamePrizeId), nameof(EntryId), IsUnique = true)]
/// <summary>
/// Class representing a prize that has been awarded due to an entry
/// hitting enough correct numbers to win a game prize.
/// </summary>
public class EntryPrize : EntityObject
{
    /// <summary>
    /// The primary key of the <see cref="Dbo.GamePrize"/> that this entry has won.
    /// </summary>
    [Required]
    public required Guid GamePrizeId { get; set; }

    /// <summary>
    /// The primary key of the <see cref="Dbo.Entry"/> that won this prize.
    /// </summary>
    [Required]
    public required Guid EntryId { get; set; }


    #region Navigation Properties
    /// <summary>
    /// The game prize that the entry won. 
    /// 
    /// Although this will always be present, it may be null if not included
    /// in the query fetching the entry prize.
    /// </summary>
    public GamePrize GamePrize { get; set; } = default!;

    /// <summary>
    /// The entry that this prize belongs to. 
    /// 
    /// Although this will always be present, it may be null if not included
    /// in the query fetching the entry prize.
    /// </summary>
    public Entry Entry { get; set; } = default!;
    #endregion
}