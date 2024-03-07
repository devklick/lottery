using System.ComponentModel.DataAnnotations;

using Lottery.DB.Entities.Base;

namespace Lottery.DB.Entities.Dbo;

public class EntryPrize : EntityObject
{
    [Required]
    public required Guid GameId { get; set; }

    [Required]
    public required Guid GamePrizeId { get; set; }

    [Required]
    public required Guid EntryId { get; set; }

    public Game Game { get; set; }
    public GamePrize GamePrize { get; set; }
    public Entry Entry { get; set; }
}