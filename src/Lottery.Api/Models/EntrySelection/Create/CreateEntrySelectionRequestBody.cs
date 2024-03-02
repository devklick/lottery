using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.EntrySelection.Create;

public class CreateEntrySelectionRequestBody : IEquatable<CreateEntrySelectionRequestBody>
{
    [Required]
    public Guid GameSelectionId { get; set; }

    public bool Equals(CreateEntrySelectionRequestBody? other)
        => other != null && other.GameSelectionId != GameSelectionId;

    public override bool Equals(object? obj)
        => Equals(obj as CreateEntrySelectionRequestBody);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(GameSelectionId);
        return hash.ToHashCode();
    }
}