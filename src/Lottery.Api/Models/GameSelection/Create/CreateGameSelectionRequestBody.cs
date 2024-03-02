using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.GameSelection.Create;

public class CreateGameSelectionRequestBody : IEquatable<CreateGameSelectionRequestBody>
{
    [Required]
    public int SelectionNumber { get; set; }

    public bool Equals(CreateGameSelectionRequestBody? other)
        => other != null && other.SelectionNumber == SelectionNumber;

    public override bool Equals(object? obj)
        => Equals(obj as CreateGameSelectionRequestBody);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(SelectionNumber);
        return hash.ToHashCode();
    }
}
