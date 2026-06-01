using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Entry.Edit;

public class EditEntryRequest
{
    [FromRoute, BindProperty(Name = "")]
    public required EditEntryRequestRoute Route { get; set; }

    [FromBody, BindProperty(Name = "")]
    public required EditEntryRequestBody Body { get; set; }
}