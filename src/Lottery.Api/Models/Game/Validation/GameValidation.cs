using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.Game.Validation;

public static class GameValidation
{
    public static class PrizeValidation
    {
        public class Prize
        {
            public int Position { get; set; }
            public int NumberMatchCount { get; set; }
        }

        public static void ValidateSequentialPositions(IEnumerable<Prize> prizes, ref List<ValidationResult> results)
        {
            var indexedPrizes = prizes.Select((p, i) => new { Prize = p, Index = i });
            var prev = 0;
            foreach (var prize in indexedPrizes.OrderBy(p => p.Prize.Position))
            {
                if (prize.Prize.Position != prev + 1)
                {
                    results.Add(new ValidationResult(
                        "Prize positions should run in sequence",
                        [string.Join('.', "Prizes", prize.Index, nameof(Prize.Position))]
                    ));
                }

                prev = prize.Prize.Position;
            }
        }

        public static void ValidateUniquePositions(IEnumerable<Prize> prizes, ref List<ValidationResult> results)
        {
            var failures = prizes.Select((prize, index) => new { Prize = prize, Index = index })
                .GroupBy(x => x.Prize.Position)
                .Where(x => x.Count() > 1);

            foreach (var failure in failures)
            {
                foreach (var other in failure)
                {
                    results.Add(new ValidationResult(
                        "Same position used multiple times",
                        [string.Join('.', "Prizes", other.Index, nameof(Prize.Position))]
                    ));
                }
            }
        }

        public static void ValidatePositionsStartFromOne(IEnumerable<Prize> prizes, ref List<ValidationResult> results)
        {
            var min = prizes.Min(p => p.Position);

            if (min != 1)
            {
                foreach (var failure in prizes.Select((p, i) => new { Prize = p, Index = i }).Where(p => p.Prize.Position == min))
                {
                    results.Add(new ValidationResult(
                        "A prize for first position is required",
                        [string.Join('.', "Prizes", failure.Index, nameof(Prize.Position))]
                    ));
                }
            }
        }

        public static void ValidateUniqueNumberMatchCount(IEnumerable<Prize> prizes, ref List<ValidationResult> results)
        {
            var failures = prizes.Select((prize, index) => new { Prize = prize, Index = index })
                .GroupBy(x => x.Prize.NumberMatchCount)
                .Where(x => x.Count() > 1);

            foreach (var failure in failures)
            {
                foreach (var other in failure)
                {
                    results.Add(new ValidationResult(
                        "Same match count used multiple times",
                        [string.Join('.', "Prizes", other.Index, nameof(Prize.Position))]
                    ));
                }
            }
        }
    }
}