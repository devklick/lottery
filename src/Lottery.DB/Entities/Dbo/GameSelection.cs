using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Entities.Base;

namespace Lottery.DB.Entities.Dbo;

[Table(nameof(GameSelection), Schema = nameof(Dbo))]
public class GameSelection : EntityObject
{
    [Required]
    public int SelectionNumber { get; set; }

    [ForeignKey(nameof(Game))]
    public Guid GameId { get; set; }

    public required Game Game { get; set; }
}