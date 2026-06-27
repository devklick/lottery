using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Mappings.Extensions;
using Lottery.Api.Models.Game.Create;
using Lottery.Api.Models.Game.Edit;
using Lottery.Api.Models.Game.Get;
using Lottery.Api.Models.Game.Result;
using Lottery.Api.Models.Game.Search;
using Lottery.Api.Repositories.Game;
using Lottery.Api.Repositories.Game.Filters;
using Lottery.Common.Models;
using Lottery.DB.Entities.Dbo;
using Lottery.DB.Entities.Idt;
using Lottery.DB.Entities.Ref;
using Lottery.Resulting;

namespace Lottery.Api.Services;

public class GameService(
    GameRepository gameRepository,
    UserService userService,
    IMapper mapper,
    ResultService resultService)
{
    public async Task<Result<CreateGameResponse>> CreateGame(CreateGameRequest request)
    {
        var userResult = await userService.GetCurrentUser();

        if (!userResult.Success)
        {
            return userResult.ChangeValue<CreateGameResponse>();
        }
        var userId = userResult.Value.Id;

        var entity = mapper.Map<Game>(request);
        entity.CreatedById = userId;

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

        var game = await gameRepository.CreateGame(entity);

        return new Result<CreateGameResponse>
        {
            Status = ResultStatus.Ok,
            Value = mapper.Map<CreateGameResponse>(game)
        };
    }

    public async Task<Result<SearchGamesResponse>> SearchGames(SearchGamesRequest request)
    {

        var (games, total) = await gameRepository.SearchGames(
            request.Query.Page,
            request.Query.Limit,
            gamesFilter: new SearchGames.GamesFilter
            {
                Name = request.Query.Name,
                GameStatus = request.Query.GameStatus,
                SortBy = new SearchGames.GamesSorting()
                {
                    Column = (SearchGames.SortCriteria)request.Query.SortBy,
                    Direction = (DB.Repositories.Common.SortDirection)request.Query.SortDirection
                },
            },
            selectionsFilter: new SearchGames.SelectionsFilter
            {
                Include = true,
                State = ItemState.Enabled,
            },
            prizesFilter: new SearchGames.PrizesFilter
            {
                Include = true,
                State = ItemState.Enabled,
            },
            resultsFilter: new SearchGames.ResultsFilter
            {
                Include = true,
                State = ItemState.Enabled,
            });

        return new Result<SearchGamesResponse>
        {
            Status = ResultStatus.Ok,
            Value = new SearchGamesResponse
            {
                Items = mapper.Map<IEnumerable<SearchGamesResponseItem>>(games),
                Limit = request.Query.Limit,
                Page = request.Query.Page,
                Total = total,
            }
        };
    }

    public async Task<Result<GetGameResponse>> GetGame(GetGameRequest request)
    {
        var game = await gameRepository.GetGame(request.Route.Id,
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
                Messages = [new() { Value = "Game not found" }]
            }
            : new Result<GetGameResponse>
            {
                Status = ResultStatus.Ok,
                Value = mapper.Map<GetGameResponse>(game)
            };
    }

    public async Task<Result<EditGameResponse>> EditGame(EditGameRequest request, ClaimsPrincipal user)
    {
        // TODO: Clean this method up, split into smaller methods
        var userIdResult = userService.GetUserId(user);
        if (userIdResult.Status != ResultStatus.Ok)
        {
            return new Result<EditGameResponse>
            {
                Messages = userIdResult.Messages,
                Status = userIdResult.Status
            };
        }

        var current = await gameRepository.GetGame(request.Route.Id,
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
            Messages = [new() { Value = "Game not found" }]
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
                Messages = [new() { Value = $"The specified changes cannot be applied while the game is in the {current.GameStatus} state" }]
            };
        }

        var entity = mapper.MergeInto<Game>(current, request.Body);


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
            var keyedSelections = entity.Selections.ToDictionary(key => key.SelectionNumber, value => value);

            for (var selectionNumber = enabledSelectionsCount + 1; selectionNumber <= request.Body.MaxSelections; selectionNumber++)
            {
                // If it exists, enable it
                if (keyedSelections.TryGetValue(selectionNumber, out GameSelection? selection))
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

        var keyedPrizes = entity.Prizes.ToDictionary(key => key.Position, value => value);
        var keyedReqPrizes = request.Body.Prizes.ToDictionary(key => key.Position, value => value);

        // For each prize that already exists
        foreach (var prize in entity.Prizes)
        {
            // if it exists on the request, update it and enable it
            if (keyedReqPrizes.TryGetValue(prize.Position, out var reqPrize))
            {
                prize.State = ItemState.Enabled;
                prize.NumberMatchCount = reqPrize.NumberMatchCount;
            }
            // otherwise, disable it.
            else prize.State = ItemState.Disabled;
        }

        // for each prize in the request
        foreach (var reqPrize in request.Body.Prizes)
        {
            // If it exists on the entity, update it and enable it
            if (keyedPrizes.TryGetValue(reqPrize.Position, out var entityPrize))
            {
                entityPrize.State = ItemState.Enabled;
                entityPrize.NumberMatchCount = reqPrize.NumberMatchCount;
            }
            // Otherwise add it
            else entity.Prizes.Add(new GamePrize
            {
                GameId = entity.Id,
                Position = reqPrize.Position,
                NumberMatchCount = reqPrize.NumberMatchCount
            });
        }

        await gameRepository.UpdateGame(entity);

        await gameRepository.SaveChangesAsync();

        // Only include enabled selections in the response
        entity.Selections = entity.Selections.Where(s => s.State == ItemState.Enabled).ToList();

        return new Result<EditGameResponse>
        {
            Status = ResultStatus.Ok,
            Value = mapper.Map<EditGameResponse>(entity)
        };
    }

    public async Task<Result<ResultGameResponse>> ResultGame(ResultGameRequest request)
    {
        var game = await gameRepository.GetGame(request.Route.GameId,
            prizesFilter: new() { Include = true, State = ItemState.Enabled },
            selectionsFilter: new() { Include = true, State = ItemState.Enabled },
            resultsFilter: new() { Include = true, State = ItemState.Enabled }
        );
        if (game == null)
        {
            return new Result<ResultGameResponse>
            {
                Status = ResultStatus.NotFound,
                Messages = [new() { Value = "Game not found" }]
            };
        }

        var result = await resultService.ResultGame(game, request.Body.WinningSelections.Select(ws => ws.SelectionNumber));

        if (result.Status != ResultStatus.Ok)
        {
            return new Result<ResultGameResponse>
            {
                Messages = result.Messages,
                Status = result.Status,
            };
        }

        return new Result<ResultGameResponse>
        {
            Status = ResultStatus.Ok,
            Value = mapper.Map<ResultGameResponse>(result.Value)
        };
    }
}