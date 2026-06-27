using Lottery.Api.Mappings;
using Lottery.Api.Models.Game.Create;
using Lottery.Api.Models.Game.Edit;
using Lottery.Api.Models.Game.Get;
using Lottery.Api.Models.Game.Result;
using Lottery.Api.Models.Game.Search;
using Lottery.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;


[ApiController]
[Route("[controller]")]
public class GameController(
    GameService gameService,
    ResultMapper resultMapper)
    : ApiControllerBase(resultMapper)
{

    [HttpGet("{id}")]
    public async Task<ActionResult<GetGameResponse>> GetGame(GetGameRequest request)
    {
        var response = await gameService.GetGame(request);

        return CreateObjectResult(response);
    }

    [Authorize(Roles = "GameAdmin,SystemAdmin")]
    [HttpPost]
    public async Task<ActionResult<CreateGameResponse>> CreateGame(CreateGameRequest request)
    {
        var response = await gameService.CreateGame(request);

        return CreateObjectResult(response);
    }

    [Authorize(Roles = "GameAdmin,SystemAdmin")]
    [HttpPost("{id}/edit")]
    public async Task<ActionResult<EditGameResponse>> EditGame(EditGameRequest request)
    {
        var response = await gameService.EditGame(request, User);

        return CreateObjectResult(response);
    }

    [HttpGet("search")]
    public async Task<ActionResult<SearchGamesResponse>> SearchGames(SearchGamesRequest request)
    {
        var response = await gameService.SearchGames(request);

        return CreateObjectResult(response);
    }

    [Authorize(Roles = "GameAdmin,SystemAdmin")]
    [HttpPost("{gameId}/result")]
    public async Task<ActionResult<ResultGameResponse>> ResultGame(ResultGameRequest request)
    {
        var response = await gameService.ResultGame(request);

        return CreateObjectResult(response);
    }
}
