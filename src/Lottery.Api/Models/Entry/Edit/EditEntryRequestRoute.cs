using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Entry.Edit;

public class EditEntryRequestRoute
{
    public required Guid EntryId { get; set; }
}