using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Models.Common;
using Lottery.Api.Models.Game.Create;
using Lottery.Api.Repositories;
using Lottery.DB.Entities.Dbo;


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
}