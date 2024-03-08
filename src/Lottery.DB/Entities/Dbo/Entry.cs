using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Entities.Base;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Entities.Dbo;

[Table(nameof(Entry), Schema = nameof(Dbo))]
// Ability to search for entries in a given game
[Index(nameof(GameId))]
// Ability to search for entries created by a given player.
[Index(nameof(CreatedById))]
/// <summary>
/// An entity representing a player entering into a game.
/// </summary>
public class Entry : EntityObject
{
    /// <summary>
    /// The primary key of the game that the entry belongs to. 
    /// 
    /// This must be provided when constructing the entity, 
    /// otherwise the insert will fail due to a missing foreign key relationship
    /// </summary>
    [Required]
    public required Guid GameId { get; set; }


    #region Navigation Properties
    /// <summary>
    /// The selections that exist on the entry
    /// </summary>
    public List<EntrySelection> Selections { get; set; } = [];

    /// <summary>
    /// The prize that has been won as a result of this entity. 
    /// 
    /// May be null if no prize has been won, or if a prize has 
    /// been won but was not included in the query returning the entry.
    /// </summary>
    public EntryPrize? Prize { get; set; }

    /// <summary>
    /// The game that the entry belongs to. 
    /// 
    /// While this will always exist, it may be null if  
    /// not included in the query returning the entry.
    /// </summary>
    public Game Game { get; set; } = default!;
    #endregion
}