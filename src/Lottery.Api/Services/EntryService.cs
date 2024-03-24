using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Models.Entry.Create;
using Lottery.DB.Entities.Dbo;
using Lottery.DB;
using Lottery.Api.Repositories;
using Lottery.Api.Models.Common;
using Lottery.Api.Models.Entry.Search;
using Lottery.Api.Repositories.Game;

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
        var game = await _gameRepository.GetGame(request.Body.GameId);

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

        var (entries, total) = await _entryRepository.SearchEntries(userIdResult.Value, request.Query.Page, request.Query.Limit);

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
}