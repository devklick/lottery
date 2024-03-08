using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Entities.Base;

namespace Lottery.DB.Entities.Dbo;

[Table(nameof(GameResult), Schema = nameof(Dbo))]
public class GameResult : EntityObject
{
    [Required]
    public required Guid GameId { get; set; }

    [Required]
    public required Guid SelectionId { get; set; }

    public Game? Game { get; set; }
    public GameSelection? Selection { get; set; }
}