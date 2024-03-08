using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Entities.Base;

namespace Lottery.DB.Entities.Dbo;

[Table(nameof(EntryPrize), Schema = nameof(Dbo))]
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