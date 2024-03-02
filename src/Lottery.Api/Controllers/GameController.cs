using Lottery.Api.Models.Game.Create;
using Lottery.Api.Services;
using Lottery.Repository.Entities.Idt;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

[Authorize(Roles = "GameAdmin,SystemAdmin")]
[ApiController]
[Route("[controller]")]
public class GameController(ILogger<GameController> logger, GameService gameService) : ControllerBase
{
    private readonly ILogger<GameController> _logger = logger;
    private readonly GameService _gameService = gameService;


    [HttpPost]
    public async Task<ActionResult<CreateGameRequest>> CreateGame(CreateGameRequest request)
    {
        await _gameService.CreateGame(request, User);
        return Ok(request);
    }
}
