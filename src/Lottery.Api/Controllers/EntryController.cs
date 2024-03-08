using Lottery.Api.Models.Entry.Create;
using Lottery.Api.Models.Entry.Get;
using Lottery.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

[Authorize(Roles = "BasicUser,GameAdmin,SystemAdmin")]
[ApiController]
[Route("[controller]")]
public class EntryController(ILogger<EntryController> logger, EntryService entryService) : ApiControllerBase
{
    private readonly ILogger<EntryController> _logger = logger;
    private readonly EntryService _entryService = entryService;

    [HttpPost]
    public async Task<ActionResult<CreateEntryResponse>> CreateEntry(CreateEntryRequest request)
    {
        var result = await _entryService.CreateEntry(request, User);

        return CreateActionResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<GetEntriesResponse>> GetEntries(GetEntriesRequest request)
    {
        var result = await _entryService.GetEntries(request, User);

        return CreateActionResult(result);
    }
}
