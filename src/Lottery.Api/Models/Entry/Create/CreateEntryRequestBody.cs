using System.ComponentModel.DataAnnotations;

using Lottery.Api.Models.EntrySelection.Create;
using Lottery.Api.Models.Validation;

namespace Lottery.Api.Models.Entry.Create;

public class CreateEntryRequestBody
{
    [Required]
    public Guid GameId { get; set; }

    [Required, UniqueValues<CreateEntrySelectionRequestBody>]
    public required List<CreateEntrySelectionRequestBody> Selections { get; set; }
}