using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Mappings.Extensions;
using Lottery.Api.Models.Common;
using Lottery.Api.Models.Game.Create;
using Lottery.Api.Models.Game.Edit;
using Lottery.Api.Models.Game.Get;
using Lottery.Api.Models.Game.Search;
using Lottery.Api.Repositories.Game;
using Lottery.Api.Repositories.Game.Filters;
using Lottery.DB.Entities.Dbo;
using Lottery.DB.Entities.Ref;

using Microsoft.Extensions.ObjectPool;


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
            request.Query.GameStates,
            sortBy: (SearchGames.SortCriteria)request.Query.SortBy,
            sortDirection: (DB.Repositories.Common.SortDirection)request.Query.SortDirection);

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
        var game = await _gameRepository.GetGame(request.Route.Id,
            selectionsFilter: new()
            {
                Include = true,
                State = ItemState.Enabled,
            }, prizesFilter: new()
            {
                Include = true,
                State = ItemState.Enabled
            }, resultsFilter: new()
            {
                Include = true,
                State = ItemState.Enabled
            });

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

    public async Task<Result<EditGameResponse>> EditGame(EditGameRequest request, ClaimsPrincipal user)
    {
        // TODO: Clean this method up, split into smaller methods
        var userIdResult = _userService.GetUserId(user);
        if (userIdResult.Status != ResultStatus.Ok)
        {
            return new Result<EditGameResponse>
            {
                Errors = userIdResult.Errors,
                Status = userIdResult.Status
            };
        }

        var current = await _gameRepository.GetGame(request.Route.Id,
            selectionsFilter: new()
            {
                Include = true,
            },
            prizesFilter: new()
            {
                Include = true,
            },
            resultsFilter: new()
            {
                Include = true,
                State = ItemState.Enabled
            });

        if (current == null) return new Result<EditGameResponse>
        {
            Status = ResultStatus.NotFound,
            Errors = [new() { Message = "Game not found" }]
        };

        // The following changes can only be applied when the game is in a future state
        var requireFutureGame = request.Body.Name != current.Name
            || request.Body.CloseTime != current.CloseTime
            || request.Body.DrawTime != current.DrawTime
            || request.Body.MaxSelections != current.Selections.Count
            || request.Body.SelectionsRequiredForEntry != current.SelectionsRequiredForEntry
            || request.Body.StartTime != current.StartTime;

        if (requireFutureGame && current.GameStatus != GameStatus.Future)
        {
            return new Result<EditGameResponse>
            {
                Status = ResultStatus.BadRequest,
                Errors = [new() { Message = $"The specified changes cannot be applied while the game is in the {current.GameStatus} state" }]
            };
        }

        var entity = _mapper.MergeInto<Game>(current, request.Body);

        var enabledSelectionsCount = entity.Selections.Count(s => s.State == ItemState.Enabled);
        // Remove any selections that are no longer required
        if (request.Body.MaxSelections < enabledSelectionsCount)
        {
            entity.Selections.ForEach(selection =>
            {
                if (selection.SelectionNumber > request.Body.MaxSelections)
                {
                    selection.State = ItemState.Disabled;
                }
            });
        }
        // Add any new selections that are required
        else if (request.Body.MaxSelections > enabledSelectionsCount)
        {
            // map the selections by selectionId. We may be able to re-enable some that are disabled
            var keyed = entity.Selections.ToDictionary(key => key.SelectionNumber, value => value);

            for (var selectionNumber = enabledSelectionsCount + 1; selectionNumber <= request.Body.MaxSelections; selectionNumber++)
            {
                // If it exists, enable it
                if (keyed.TryGetValue(selectionNumber, out GameSelection? selection))
                {
                    selection.State = ItemState.Enabled;
                }
                // Otherwise add it
                else entity.Selections.Add(new GameSelection
                {
                    SelectionNumber = selectionNumber,
                    CreatedById = userIdResult.Value
                });

            }
        }

        await _gameRepository.UpdateGame(entity);

        // Only include enabled selections in the response
        entity.Selections = entity.Selections.Where(s => s.State == ItemState.Enabled).ToList();

        return new Result<EditGameResponse>
        {
            Status = ResultStatus.Ok,
            Value = _mapper.Map<EditGameResponse>(entity)
        };
    }
}