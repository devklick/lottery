using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Lottery.Api.Models.Game.Validation;

using Lottery.Api.Models.Validation;
using Lottery.DB.Entities.Ref;

namespace Lottery.Api.Models.Game.Edit;

public class EditGameRequestBody : IValidatableObject
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

    [Required]
    public required List<Prize> Prizes { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        ValidatePrizesNumberMatchCount(ref results);

        var prizes = Prizes.Select(p => (GameValidation.PrizeValidation.Prize)p);

        GameValidation.PrizeValidation.ValidateUniquePositions(prizes, ref results);
        GameValidation.PrizeValidation.ValidatePositionsStartFromOne(prizes, ref results);
        GameValidation.PrizeValidation.ValidateSequentialPositions(prizes, ref results);
        GameValidation.PrizeValidation.ValidateUniqueNumberMatchCount(prizes, ref results);

        return results;
    }

    private void ValidatePrizesNumberMatchCount(ref List<ValidationResult> results)
    {
        var prizes = Prizes.Select((p, i) => new { Prize = p, Index = i });

        foreach (var failure in prizes.Where(p => p.Prize.NumberMatchCount > SelectionsRequiredForEntry))
        {
            results.Add(new ValidationResult(
                "Match count cannot be greater than the number of selections in an entry",
                [string.Join('.', nameof(Prizes), failure.Index, nameof(Prize.NumberMatchCount))]
            ));
        }
    }

    public class Prize
    {
        [Required]
        public int Position { get; set; }

        [Required]
        public int NumberMatchCount { get; set; }

        public static implicit operator GameValidation.PrizeValidation.Prize(Prize prize)
            => new() { NumberMatchCount = prize.NumberMatchCount, Position = prize.Position };
    }
}