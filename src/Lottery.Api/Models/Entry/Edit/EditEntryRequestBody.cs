using System.ComponentModel.DataAnnotations;

using Lottery.Api.Models.Validation;

namespace Lottery.Api.Models.Entry.Edit;

public class EditEntryRequestBody
{
    [Required, UniqueValues<Selection>]
    public required List<Selection> Selections { get; set; }

    public class Selection : IEquatable<Selection>
    {
        public static Selection Create(int selectionNumber)
            => new() { SelectionNumber = selectionNumber };

        [Required]
        public int SelectionNumber { get; set; }

        public bool Equals(Selection? other)
            => other != null && other.SelectionNumber != SelectionNumber;

        public override bool Equals(object? obj)
            => Equals(obj as Selection);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(SelectionNumber);
            return hash.ToHashCode();
        }
    }
}