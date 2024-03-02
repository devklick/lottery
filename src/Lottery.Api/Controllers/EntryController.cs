using Lottery.Api.Models.Entry.Create;
using Lottery.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

[Authorize(Roles = "BasicUser,GameAdmin,SystemAdmin")]
[ApiController]
[Route("[controller]")]
public class EntryController(ILogger<EntryController> logger, EntryService entryService) : ControllerBase
{
    private readonly ILogger<EntryController> _logger = logger;
    private readonly EntryService _entryService = entryService;

    [HttpPost]
    public async Task<ActionResult<object>> CreateEntry(CreateEntryRequest request)
    {
        await _entryService.CreateEntry(request, User);
        return Ok();
    }
}
