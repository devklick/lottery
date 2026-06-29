using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Models.Entry.Create;
using Lottery.DB.Entities.Dbo;
using Lottery.Api.Models.Common;
using Lottery.Api.Models.Entry.Search;
using Lottery.Api.Repositories.Game;
using Lottery.Api.Models.Entry.Edit;
using Lottery.Api.Repositories.Entry;
using Lottery.Api.Repositories.Entry.Filters;
using Lottery.DB.Entities.Ref;
using Lottery.Common.Models;
using Lottery.DB.Entities.Idt;

namespace Lottery.Api.Services;

public class EntryService(
    EntryRepository entryRepository,
    GameRepository gameRepository,
    UserService userService,
    IMapper mapper,
    TimeProvider timeProvider)
{
    public async Task<Result<CreateEntryResponse>> CreateEntry(CreateEntryRequest request)
    {
        // Grab the user Id that's creating the entry
        var userResult = await userService.GetCurrentUser();

        if (!userResult.Success)
        {
            return userResult.ChangeValue<CreateEntryResponse>();
        }

        var user = userResult.Value;

        // Grab the game to be sure it exists
        var game = await gameRepository.GetGame(request.Body.GameId,
            selectionsFilter: new() { Include = true },
            prizesFilter: new() { Include = true },
            resultsFilter: new() { Include = true });

        if (game == null)
        {
            return Result<CreateEntryResponse>.Error(
                ResultStatus.NotFound,
                $"Unable to locate game with gameId {request.Body.GameId}");
        }

        if (game.CloseTime <= timeProvider.GetUtcNow())
        {
            return Result<CreateEntryResponse>.Error(
                ResultStatus.BadRequest,
                $"Game {request.Body.GameId} is closed");
        }

        // Make sure the correct number of selections are present on the entry
        if (request.Body.Selections.Count != game.SelectionsRequiredForEntry)
        {
            return Result<CreateEntryResponse>.Error(
                ResultStatus.BadRequest,
                $"Expected {game.SelectionsRequiredForEntry} selections, found {request.Body.Selections.Count}");
        }

        var entries = await entryRepository.SearchEntries(1, 1,
            entryFilter: new() { UserId = user.Id }
        );

        if (entries.Total >= game.MaxEntriesPerPlayer)
        {
            return Result<CreateEntryResponse>
                .Error(ResultStatus.BadRequest)
                .AddMessages(MessageCode.MaxEntriesReached, $"Player already has {entries.Total} entries in this game");
        }

        // convert the entry request to an entry entity
        var entry = mapper.Map<Entry>(request);
        entry.CreatedById = user.Id;
        entry.Selections = [];

        // We need to look up the game selections using the entry selection numbers
        foreach (var es in request.Body.Selections)
        {
            var gs = game.Selections.Find(gs => gs.SelectionNumber == es.SelectionNumber);
            if (gs == null)
            {
                return new Result<CreateEntryResponse>
                {
                    Status = ResultStatus.BadRequest,
                    Messages = [new() { Value = $"Unable to find game selection with selection number {es.SelectionNumber}" }]
                };
            }
            entry.Selections.Add(new EntrySelection
            {
                GameSelectionId = gs.Id,
                CreatedById = entry.CreatedById,
                State = entry.State,
                EntryId = entry.Id
            });
        }

        entry.Selections.ForEach(s =>
        {
            s.CreatedById = entry.CreatedById;
            s.State = entry.State;
        });

        var result = await entryRepository.CreateEntry(entry);

        return Result<CreateEntryResponse>.Ok(new()
        {
            Id = result.Id
        });
    }

    public async Task<Result<SearchEntriesResponse>> SearchEntries(SearchEntriesRequest request)
    {
        var userIdResult = await userService.GetCurrentUser();
        if (!userIdResult.Success)
        {
            return new Result<SearchEntriesResponse>
            {
                Messages = userIdResult.Messages,
                Status = userIdResult.Status
            };
        }

        var (entries, total) = await entryRepository.SearchEntries(
            request.Query.Page, request.Query.Limit,
            entryFilter: new SearchEntries.EntryFilter
            {
                UserId = userIdResult.Value.Id,
                State = ItemState.Enabled
            },
            gameFilter: new SearchEntries.GameFilter
            {
                GameId = request.Query.GameId,
            },
            selectionsFilter: new SearchEntries.SelectionsFilter
            {
                Include = true,
                State = ItemState.Enabled
            },
            prizeFilter: new SearchEntries.PrizeFilter
            {
                Include = true,
                State = ItemState.Enabled
            });

        return new Result<SearchEntriesResponse>
        {
            Status = ResultStatus.Ok,
            Value = new SearchEntriesResponse
            {
                Items = mapper.Map<IEnumerable<SearchEntriesResponseItem>>(entries),
                Limit = request.Query.Limit,
                Page = request.Query.Page,
                Total = total,
            }
        };
    }

    public async Task<Result<EditEntryResponse>> EditEntry(EditEntryRequest request)
    {
        // get the player
        var userIdResult = await userService.GetCurrentUser();
        if (!userIdResult.Success)
        {
            return new Result<EditEntryResponse>
            {
                Status = userIdResult.Status,
                Messages = userIdResult.Messages
            };
        }

        // get the players entry
        var entry = await entryRepository.GetEntry(request.Route.EntryId,
            gamesFilter: new GetEntry.GameFilter
            {
                Include = true,
                SelectionFilter = new GetEntry.GameFilter.GameSelectionFilter
                {
                    Include = true,
                    State = ItemState.Enabled
                }
            },
            selectionsFilter: new GetEntry.SelectionsFilter
            {
                Include = true
            });

        // Make sure it's a valid entry for this player
        if (entry == null || entry.CreatedById != userIdResult.Value.Id)
        {
            return new Result<EditEntryResponse>
            {
                Status = ResultStatus.NotFound,
                Messages = [new() { Value = "Unable to find the players entry" }]
            };
        }

        // Make sure the requets has the right number of selections
        if (entry.Game.SelectionsRequiredForEntry != request.Body.Selections.Count)
        {
            return new Result<EditEntryResponse>
            {
                Status = ResultStatus.BadRequest,
                Messages = [new() { Value = $"Expected {entry.Game.SelectionsRequiredForEntry} selections, found {request.Body.Selections.Count}" }]
            };
        }

        // Disable all the current selections
        foreach (var selection in entry.Selections)
        {
            selection.State = ItemState.Disabled;
        }

        var indexedEs = entry.Selections.ToDictionary(key => key.GameSelection.SelectionNumber, value => value);
        var indexedGs = entry.Game.Selections.ToDictionary(key => key.SelectionNumber, value => value);

        foreach (var selection in request.Body.Selections)
        {
            // If an entry selection already exists for this selection number, enable it
            if (indexedEs.TryGetValue(selection.SelectionNumber, out var es))
            {
                es.State = ItemState.Enabled;
            }
            // If it doesnt exist, add it
            else if (indexedGs.TryGetValue(selection.SelectionNumber, out var gs))
            {
                entry.Selections.Add(new EntrySelection
                {
                    EntryId = entry.Id,
                    CreatedById = entry.CreatedById,
                    GameSelectionId = gs.Id,
                });
            }
            // If it's a selection number that doesnt exist on the game, return error
            else return new Result<EditEntryResponse>
            {
                Status = ResultStatus.BadRequest,
                Messages = [new() { Value = $"No selection selection exists for selection number {selection.SelectionNumber}" }]
            };
        }

        await entryRepository.UpdateEntry(entry);

        // The selections list currently includes disabled ones. 
        // Inlcude only enabled entry selections in the response
        entry.Selections = entry.Selections.Where(s => s.State == ItemState.Enabled).ToList();

        return new Result<EditEntryResponse>
        {
            Status = ResultStatus.Ok,
            Value = mapper.Map<EditEntryResponse>(entry)
        };
    }
}