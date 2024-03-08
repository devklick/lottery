using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Entities.Base;

namespace Lottery.DB.Entities.Dbo;

[Table(nameof(Entry), Schema = nameof(Dbo))]
public class Entry : EntityObject
{
    public Guid GameId { get; set; }
    public required List<EntrySelection> Selections { get; set; }
    public EntryPrize? Prize { get; set; }
    public Game Game { get; set; }
}