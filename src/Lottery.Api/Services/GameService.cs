using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Models.Game.Create;
using Lottery.Api.Repositories;
using Lottery.DB.Entities.Dbo;


namespace Lottery.Api.Services;

public class GameService(GameRepository gameRepository, UserService userService, IMapper mapper)
{
    private readonly GameRepository _gameRepository = gameRepository;
    private readonly UserService _userManager = userService;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateGameResponse> CreateGame(CreateGameRequest request, ClaimsPrincipal user)
    {
        request.Unbound.CreatedById = _userManager.GetUserId(user)
            ?? throw new Exception("UserId not found");

        var entity = _mapper.Map<Game>(request);

        entity.Selections.ForEach(s =>
        {
            s.CreatedById = entity.CreatedById;
            s.State = entity.State;
        });

        entity.Prizes.ForEach(p =>
        {
            p.CreatedById = entity.CreatedById;
            p.State = entity.State;
        });

        var game = await _gameRepository.CreateGame(entity);

        return new CreateGameResponse
        {
            Body = _mapper.Map<CreateGameResponseBody>(game)
        };
    }
}