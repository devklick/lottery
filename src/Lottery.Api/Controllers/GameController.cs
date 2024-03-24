using Lottery.Api.Models.Game.Create;
using Lottery.Api.Models.Game.Get;
using Lottery.Api.Models.Game.Search;
using Lottery.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;


[ApiController]
[Route("[controller]")]
public class GameController(GameService gameService) : ApiControllerBase
{
    private readonly GameService _gameService = gameService;

    [HttpGet("{id}")]
    public async Task<ActionResult<GetGameResponse>> GetGame(GetGameRequest request)
    {
        var response = await _gameService.GetGame(request);

        return CreateActionResult(response);
    }

    [Authorize(Roles = "GameAdmin,SystemAdmin")]
    [HttpPost]
    public async Task<ActionResult<CreateGameResponse>> CreateGame(CreateGameRequest request)
    {
        var response = await _gameService.CreateGame(request, User);

        return CreateActionResult(response);
    }

    [HttpGet("search")]
    public async Task<ActionResult<SearchGamesResonse>> SearchGames(SearchGamesRequest request)
    {
        var response = await _gameService.SearchGames(request);

        return CreateActionResult(response);
    }
}
