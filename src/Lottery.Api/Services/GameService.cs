using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Models.Common;
using Lottery.Api.Models.Game.Create;
using Lottery.Api.Models.Game.Get;
using Lottery.Api.Models.Game.Search;
using Lottery.Api.Repositories;
using Lottery.Api.Repositories.Game;
using Lottery.DB.Entities.Dbo;
using Lottery.DB.Repository;


namespace Lottery.Api.Services;

public class GameService(GameRepository gameRepository, UserService userService, IMapper mapper)
{
    private readonly GameRepository _gameRepository = gameRepository;
    private readonly UserService _userService = userService;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CreateGameResponse>> CreateGame(CreateGameRequest request, ClaimsPrincipal user)
    {
        var userIdResult = _userService.GetUserId(user);

        if (userIdResult.Status != ResultStatus.Ok)
        {
            return new Result<CreateGameResponse>
            {
                Errors = userIdResult.Errors,
                Status = userIdResult.Status
            };
        };

        request.Unbound.CreatedById = userIdResult.Value;

        var entity = _mapper.Map<Game>(request);

        for (int i = 1; i <= request.Body.MaxSelections; i++)
        {
            entity.Selections.Add(new GameSelection
            {
                SelectionNumber = i,
                CreatedById = entity.CreatedById,
                State = entity.State,
            });
        }

        entity.Prizes.ForEach(p =>
        {
            p.CreatedById = entity.CreatedById;
            p.State = entity.State;
        });

        var game = await _gameRepository.CreateGame(entity);

        return new Result<CreateGameResponse>
        {
            Status = ResultStatus.Ok,
            Value = _mapper.Map<CreateGameResponse>(game)
        };
    }

    public async Task<Result<SearchGamesResonse>> SearchGames(SearchGamesRequest request)
    {

        var (games, total) = await _gameRepository.SearchGames(
            request.Query.Page,
            request.Query.Limit,
            name: request.Query.Name,
            request.Query.GameStates.Select(s => (SearchGamesInState)s).ToList(),
            sortBy: (DB.Repository.SearchGamesSortCriteria)request.Query.SortBy,
            sortDirection: (DB.Repository.Common.SortDirection)request.Query.SortDirection);

        return new Result<SearchGamesResonse>
        {
            Status = ResultStatus.Ok,
            Value = new SearchGamesResonse
            {
                Items = _mapper.Map<IEnumerable<SearchGamesResponseItem>>(games),
                Limit = request.Query.Limit,
                Page = request.Query.Page,
                Total = total,
            }
        };
    }

    public async Task<Result<GetGameResponse>> GetGame(GetGameRequest request)
    {
        var game = await _gameRepository.GetGame(request.Route.Id);

        return game == null
            ? new Result<GetGameResponse>
            {
                Status = ResultStatus.NotFound,
                Errors = [new() { Message = "Game not found" }]
            }
            : new Result<GetGameResponse>
            {
                Status = ResultStatus.Ok,
                Value = _mapper.Map<GetGameResponse>(game)
            };
    }
}