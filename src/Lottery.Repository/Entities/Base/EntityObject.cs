using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lottery.Repository.Attributes;
using Lottery.Repository.Entities.Dbo;
using Lottery.Repository.Entities.Idt;
using Lottery.Repository.Entities.Ref;

namespace Lottery.Repository.Entities.Base;

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
    public DateTime StateLastUpdated { get; set; }

    public required AppUser CreatedBy { get; set; }
}