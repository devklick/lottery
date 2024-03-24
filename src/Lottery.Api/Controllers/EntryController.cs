using Lottery.Api.Models.Entry.Create;
using Lottery.Api.Models.Entry.Search;
using Lottery.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

[Authorize(Roles = "BasicUser,GameAdmin,SystemAdmin")]
[ApiController]
[Route("[controller]")]
public class EntryController(EntryService entryService) : ApiControllerBase
{
    private readonly EntryService _entryService = entryService;

    [HttpPost]
    public async Task<ActionResult<CreateEntryResponse>> CreateEntry(CreateEntryRequest request)
    {
        var result = await _entryService.CreateEntry(request, User);

        return CreateActionResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<SearchEntriesResponse>> SearchEntries(SearchEntriesRequest request)
    {
        var result = await _entryService.SearchEntries(request, User);

        return CreateActionResult(result);
    }
}
