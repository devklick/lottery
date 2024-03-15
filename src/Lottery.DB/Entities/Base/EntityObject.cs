using System.ComponentModel.DataAnnotations;

using Lottery.DB.Attributes;
using Lottery.DB.Entities.Idt;
using Lottery.DB.Entities.Ref;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Entities.Base;

[Index(nameof(CreatedById))]
/// <summary>
/// The base object which all entities should extend.
/// </summary>
public abstract class EntityObject
{
    /// <summary>
    /// The primary key for the entity. Can be specified in the app layer or
    /// left unspecified, in which case the database will generate one
    /// </summary>
    [Key, SqlColumnDefaultConstraint("gen_random_uuid()", isSqlCommand: true)]
    public Guid Id { get; set; }

    /// <summary>
    /// The date and time that the entity was created.
    ///
    /// This does not ned to be specified when constructing the entity as
    /// the DBContext will take care of applying the value.
    /// </summary>
    [Required, SqlColumnDefaultConstraint("CURRENT_TIMESTAMP", isSqlCommand: true)]
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// The Id of the user who created the entity. 
    /// 
    /// This must be provided when constructing the entity, otherwise
    /// the insert will fail due to a missing foreign key relationship.
    /// </summary> 
    [Required]
    public Guid CreatedById { get; set; }

    /// <summary>
    /// The current state of the entity. 
    /// 
    /// Can be included or omitted when constructing the entity.
    /// The default value is <see cref="ItemState.Enabled"/>
    /// </summary>
    [Required]
    [SqlColumnDefaultConstraint(ItemState.Enabled)]
    public ItemState State { get; set; } = ItemState.Enabled;

    /// <summary>
    /// The date and time that the <see cref="State"/> was last updated.
    /// 
    /// This does not have to be specified manually, as the DBContext
    /// will take care of updating this whenever the State changes.
    /// </summary>
    [Required, SqlColumnDefaultConstraint("CURRENT_TIMESTAMP", isSqlCommand: true)]
    public DateTime StateLastUpdatedUtc { get; set; }

    /// <summary>
    /// The date and time that the entity was last updated.
    /// 
    /// This does not have to be specified manually, as the DBContext
    /// will take care of updating this whenever the State changes.
    /// </summary>
    [Required, SqlColumnDefaultConstraint("CURRENT_TIMESTAMP", isSqlCommand: true)]
    public DateTime UpdatedOnUtc { get; set; }

    #region Navigation Properties
    /// <summary>
    /// Navigation property to the user who created the entity. 
    /// 
    /// While this will always exist, it may be null if it's not included
    /// in the query being executed.
    /// </summary>
    public AppUser CreatedBy { get; set; } = default!;
    #endregion
}