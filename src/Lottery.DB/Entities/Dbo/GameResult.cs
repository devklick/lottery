using System.ComponentModel.DataAnnotations;

using Lottery.DB.Entities.Base;

namespace Lottery.DB.Entities.Dbo;

public class GameResult : EntityObject
{
    [Required]
    public required Guid GameId { get; set; }

    [Required]
    public required Guid SelectionId { get; set; }

    public required Game Game { get; set; }
    public required GameSelection Selection { get; set; }
}