using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lottery.Repository.Entities.Base;

namespace Lottery.Repository.Entities.Dbo;

[Table(nameof(EntrySelection), Schema = nameof(Dbo))]
public class EntrySelection : EntityObject
{
    [Required, ForeignKey(nameof(Entry))]
    public Guid EntryId { get; set; }

    [Required, ForeignKey(nameof(GameSelection))]
    public Guid GameSelectionId { get; set; }

    public required Entry Entry { get; set; }
    public required GameSelection GameSelection { get; set; }
    public int SelectionNumber => GameSelection.SelectionNumber;
}