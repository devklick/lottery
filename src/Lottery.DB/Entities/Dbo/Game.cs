using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Entities.Base;

namespace Lottery.DB.Entities.Dbo;

[Table(nameof(Game), Schema = nameof(Dbo))]
public class Game : EntityObject
{
    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime DrawTime { get; set; }

    [Required, StringLength(64)]
    public required string Name { get; set; }

    [Required]
    public required int NumbersRequired { get; set; }

    public List<GameSelection> Selections { get; set; }
    public List<GamePrize> Prizes { get; set; }
    public List<Entry> Entries { get; set; }
}