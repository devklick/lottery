using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Lottery.Api.Models.Validation;
using Lottery.DB.Entities.Ref;

namespace Lottery.Api.Models.Game.Edit;

public class EditGameRequestBody
{
    [Required, CompareWithOther(ComparisonType.LessThan, nameof(CloseTime))]
    public required DateTime StartTime { get; set; }

    [Required, CompareWithOther(ComparisonType.LessThanOrEqual, nameof(CloseTime))]
    public required DateTime CloseTime { get; set; }

    [Required]
    public required DateTime DrawTime { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required, CompareWithOther(ComparisonType.LessThanOrEqual, nameof(MaxSelections))]
    public int SelectionsRequiredForEntry { get; set; }

    [Required, Range(0, 100)]
    public int? MaxSelections { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ItemState State { get; set; }

    // TODO: Support editing prizes

    public class Prize
    {
        [Required,]
        public int Position { get; set; }
        [Required]
        public int NumberCountMatch { get; set; }
    }
}