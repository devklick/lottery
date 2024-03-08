using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.EntrySelection.Create;

public class CreateEntrySelectionRequestBody : IEquatable<CreateEntrySelectionRequestBody>
{
    [Required]
    public int SelectionNumber { get; set; }

    public bool Equals(CreateEntrySelectionRequestBody? other)
        => other != null && other.SelectionNumber != SelectionNumber;

    public override bool Equals(object? obj)
        => Equals(obj as CreateEntrySelectionRequestBody);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(SelectionNumber);
        return hash.ToHashCode();
    }
}