using System.ComponentModel.DataAnnotations;

using Lottery.Api.Models.Validation;

namespace Lottery.Api.Models.Game.Result;

public class ResultGameRequestBody
{
    [UniqueValues<GameSelection>()]
    public List<GameSelection> WinningSelections { get; set; } = [];

    public class GameSelection : IEquatable<GameSelection>
    {
        public static GameSelection Create(int selectionNumber)
            => new() { SelectionNumber = selectionNumber };

        [Required, Range(0, 100)]
        public required int SelectionNumber { get; set; }

        public bool Equals(GameSelection? other)
            => other != null && other.SelectionNumber == SelectionNumber;
    }
}