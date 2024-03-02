using System.ComponentModel.DataAnnotations.Schema;
using Lottery.Repository.Entities.Base;

namespace Lottery.Repository.Entities.Dbo;

[Table(nameof(Entry), Schema = nameof(Dbo))]
public class Entry : EntityObject
{
    public required List<EntrySelection> Selections { get; set; }
}