using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Lottery.Api.Models.Validation;
using Lottery.DB.Entities.Ref;

namespace Lottery.Api.Models.Game.Create;

public class CreateGameRequestBody
{
    [Required]
    public required DateTime StartTime { get; set; }

    [Required]
    public required DateTime CloseTime { get; set; }

    [Required]
    public required DateTime DrawTime { get; set; }

    [Required, StringLength(64)]
    public required string Name { get; set; }

    [DefaultValue(ItemState.Enabled)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ItemState State { get; set; }

    /// <summary>
    /// The numbers of selections to generate for this game. 
    /// </summary>
    [Required, Range(0, 100)]
    public required int MaxSelections { get; set; }

    /// <summary>
    /// The number of selections a player must have when entering the game
    /// </summary>
    [Required, CompareWithOther(ComparisonType.LessThanOrEqual, nameof(MaxSelections))]
    public required int SelectionsRequiredForEntry { get; set; }


    [Required]
    public required List<Prize> Prizes { get; set; }

    public class Prize
    {
        [Required]
        public int Position { get; set; }

        [Required]
        public int NumberMatchCount { get; set; }
    }
}