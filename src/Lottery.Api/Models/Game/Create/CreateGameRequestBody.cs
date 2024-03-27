using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Lottery.Api.Models.Validation;
using Lottery.DB.Entities.Ref;

namespace Lottery.Api.Models.Game.Create;

public class CreateGameRequestBody : IValidatableObject
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
    public required PrizeList Prizes { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        ValidatePrizesNumberMatchCount(ref results);

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
    }

    public class PrizeList : List<Prize>, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            ValidateUniquePositions(ref results);
            ValidatePositionsStartFromOne(ref results);
            ValidateSequentialPositions(ref results);
            ValidateUniqueNumberMatchCount(ref results);

            return results;
        }

        private void ValidateSequentialPositions(ref List<ValidationResult> results)
        {
            var prizes = this.Select((p, i) => new { Prize = p, Index = i });
            var prev = 0;
            foreach (var prize in prizes.OrderBy(p => p.Prize.Position))
            {
                if (prize.Prize.Position != prev + 1)
                {
                    results.Add(new ValidationResult(
                        "Prize positions should run in sequence",
                        [string.Join('.', nameof(Prizes), prize.Index, nameof(Prize.Position))]
                    ));
                }

                prev = prize.Prize.Position;
            }
        }

        private void ValidateUniquePositions(ref List<ValidationResult> results)
        {
            var failures = this.Select((prize, index) => new { Prize = prize, Index = index })
                .GroupBy(x => x.Prize.Position)
                .Where(x => x.Count() > 1);

            foreach (var failure in failures)
            {
                foreach (var other in failure)
                {
                    results.Add(new ValidationResult(
                        "Same position used multiple times",
                        [string.Join('.', nameof(Prizes), other.Index, nameof(Prize.Position))]
                    ));
                }
            }
        }

        private void ValidatePositionsStartFromOne(ref List<ValidationResult> results)
        {
            var min = this.Min(p => p.Position);

            if (min != 1)
            {
                foreach (var failure in this.Select((p, i) => new { Prize = p, Index = i }).Where(p => p.Prize.Position == min))
                {
                    results.Add(new ValidationResult(
                        "A prize for first position is required",
                        [string.Join('.', nameof(Prizes), failure.Index, nameof(Prize.Position))]
                    ));
                }
            }
        }

        private void ValidateUniqueNumberMatchCount(ref List<ValidationResult> results)
        {
            var failures = this.Select((prize, index) => new { Prize = prize, Index = index })
                .GroupBy(x => x.Prize.NumberMatchCount)
                .Where(x => x.Count() > 1);

            foreach (var failure in failures)
            {
                foreach (var other in failure)
                {
                    results.Add(new ValidationResult(
                        "Same match count used multiple times",
                        [string.Join('.', nameof(Prizes), other.Index, nameof(Prize.Position))]
                    ));
                }
            }
        }
    }
}