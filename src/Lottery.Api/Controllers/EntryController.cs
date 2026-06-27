using Lottery.Api.Mappings;
using Lottery.Api.Models.Entry.Create;
using Lottery.Api.Models.Entry.Edit;
using Lottery.Api.Models.Entry.Search;
using Lottery.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

[Authorize(Roles = "BasicUser,GameAdmin,SystemAdmin")]
[ApiController]
[Route("[controller]")]
public class EntryController(
    EntryService entryService,
    ResultMapper resultMapper)
    : ApiControllerBase(resultMapper)
{
    [HttpPost]
    public async Task<ActionResult<CreateEntryResponse>> CreateEntry(CreateEntryRequest request)
    {
        var result = await entryService.CreateEntry(request);

        return CreateObjectResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<SearchEntriesResponse>> SearchEntries(SearchEntriesRequest request)
    {
        var result = await entryService.SearchEntries(request, User);

        return CreateObjectResult(result);
    }

    [HttpPost("{entryId}/edit")]
    public async Task<ActionResult<EditEntryResponse>> EditEntry(EditEntryRequest request)
    {
        var result = await entryService.EditEntry(request, User);

        return CreateObjectResult(result);
    }
}
