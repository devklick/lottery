using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Attributes;
using Lottery.DB.Entities.Idt;
using Lottery.DB.Entities.Ref;

namespace Lottery.DB.Entities.Base;

[SqlTableIndex(nameof(CreatedById))]
public abstract class EntityObject
{
    [Key]
    public Guid Id { get; set; }

    [Required, SqlColumnDefaultConstraint("CURRENT_TIMESTAMP", isSqlCommand: true)]
    public DateTime CreatedOnUtc { get; set; }

    [Required, ForeignKey(nameof(CreatedBy))]
    public Guid CreatedById { get; set; }

    [Required]
    [SqlColumnDefaultConstraint(ItemState.Enabled)]
    public ItemState State { get; set; }

    [Required, SqlColumnDefaultConstraint("CURRENT_TIMESTAMP", isSqlCommand: true)]
    public DateTime StateLastUpdatedUtc { get; set; }

    [Required, SqlColumnDefaultConstraint("CURRENT_TIMESTAMP", isSqlCommand: true)]
    public DateTime UpdatedOnUtc { get; set; }

    public AppUser CreatedBy { get; set; }
}