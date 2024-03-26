using System.ComponentModel.DataAnnotations;

using Lottery.Api.Models.Validation;

namespace Lottery.Api.Models.Entry.Create;

public class CreateEntryRequestBody
{
    [Required]
    public Guid GameId { get; set; }

    [Required, UniqueValues<Selection>]
    public required List<Selection> Selections { get; set; }

    public class Selection : IEquatable<Selection>
    {
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