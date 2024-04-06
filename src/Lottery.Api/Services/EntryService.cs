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

namespace Lottery.Api.Services;

public class EntryService(EntryRepository entryRepository, GameRepository gameRepository, UserService userService, IMapper mapper)
{
    private readonly UserService _userService = userService;
    private readonly IMapper _mapper = mapper;
    private readonly EntryRepository _entryRepository = entryRepository;
    private readonly GameRepository _gameRepository = gameRepository;

    public async Task<Result<CreateEntryResponse>> CreateEntry(CreateEntryRequest request, ClaimsPrincipal user)
    {
        // Grab the user Id that's creating the entry
        var userIdResult = _userService.GetUserId(user);

        if (userIdResult.Status != ResultStatus.Ok)
        {
            return new Result<CreateEntryResponse>
            {
                Status = userIdResult.Status,
                Errors = userIdResult.Errors
            };
        }

        request.Unbound.CreatedById = userIdResult.Value;

        // Grab the game to be sure it exists
        var game = await _gameRepository.GetGame(request.Body.GameId,
            selectionsFilter: new()
            {
                Include = true
            },
            prizesFilter: new()
            {
                Include = true
            },
            resultsFilter: new()
            {
                Include = true
            });

        if (game == null)
        {
            return new Result<CreateEntryResponse>
            {
                Status = ResultStatus.NotFound,
                Errors = [new() { Message = $"Unable to locate game with gameId {request.Body.GameId}" }]
            };
        }

        // Make sure the correct number of selections are present on the entry
        if (request.Body.Selections.Count != game.SelectionsRequiredForEntry)
        {
            return new Result<CreateEntryResponse>
            {
                Status = ResultStatus.BadRequest,
                Errors = [new() { Message = $"Expected {game.SelectionsRequiredForEntry} selections, found {request.Body.Selections.Count}" }]
            };
        }

        // convert the entry request to an entry entity
        var entry = _mapper.Map<Entry>(request);
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
                    Errors = [new() { Message = $"Unable to find game selection with selection number {es.SelectionNumber}" }]
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

        var result = await _entryRepository.CreateEntry(entry);

        return new Result<CreateEntryResponse>
        {
            Status = ResultStatus.Ok,
            Value = new CreateEntryResponse()
        };
    }

    public async Task<Result<SearchEntriesResponse>> SearchEntries(SearchEntriesRequest request, ClaimsPrincipal user)
    {
        var userIdResult = _userService.GetUserId(user);
        if (userIdResult.Status != ResultStatus.Ok)
        {
            return new Result<SearchEntriesResponse>
            {
                Errors = userIdResult.Errors,
                Status = userIdResult.Status
            };
        }

        var (entries, total) = await _entryRepository.SearchEntries(
            request.Query.Page, request.Query.Limit,
            entryFilter: new SearchEntries.EntryFilter
            {
                UserId = userIdResult.Value,
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
                Items = _mapper.Map<IEnumerable<SearchEntriesResponseItem>>(entries),
                Limit = request.Query.Limit,
                Page = request.Query.Page,
                Total = total,
            }
        };
    }

    public async Task<Result<EditEntryResponse>> EditEntry(EditEntryRequest request, ClaimsPrincipal user)
    {
        // get the player
        var userIdResult = _userService.GetUserId(user);
        if (userIdResult.Status != ResultStatus.Ok)
        {
            return new Result<EditEntryResponse>
            {
                Status = userIdResult.Status,
                Errors = userIdResult.Errors
            };
        }

        // get the players entry
        var entry = await _entryRepository.GetEntry(request.Route.EntryId,
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
        if (entry == null || entry.CreatedById != userIdResult.Value)
        {
            return new Result<EditEntryResponse>
            {
                Status = ResultStatus.NotFound,
                Errors = [new() { Message = "Unable to find the players entry" }]
            };
        }

        // Make sure the requets has the right number of selections
        if (entry.Game.SelectionsRequiredForEntry != request.Body.Selections.Count)
        {
            return new Result<EditEntryResponse>
            {
                Status = ResultStatus.BadRequest,
                Errors = [new() { Message = $"Expected {entry.Game.SelectionsRequiredForEntry} selections, found {request.Body.Selections.Count}" }]
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
                Errors = [new() { Message = $"No selection selection exists for selection number {selection.SelectionNumber}" }]
            };
        }

        await _entryRepository.UpdateEntry(entry);

        // The selections list currently includes disabled ones. 
        // Inlcude only enabled entry selections in the response
        entry.Selections = entry.Selections.Where(s => s.State == ItemState.Enabled).ToList();

        return new Result<EditEntryResponse>
        {
            Status = ResultStatus.Ok,
            Value = _mapper.Map<EditEntryResponse>(entry)
        };
    }
}