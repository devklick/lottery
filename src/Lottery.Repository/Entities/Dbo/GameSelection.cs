using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lottery.Repository.Entities.Base;

namespace Lottery.Repository.Entities.Dbo;

[Table(nameof(GameSelection), Schema = nameof(Dbo))]
public class GameSelection : EntityObject
{
    [Required]
    public int SelectionNumber { get; set; }

    [ForeignKey(nameof(Game))]
    public Guid GameId { get; set; }

    public required Game Game { get; set; }
}