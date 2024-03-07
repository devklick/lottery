using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Models.Entry.Create;
using Lottery.DB.Entities.Dbo;
using Lottery.DB;
using Lottery.Api.Repositories;

namespace Lottery.Api.Services;

public class EntryService(EntryRepository entryRepository, UserService userService, IMapper mapper)
{
    private readonly UserService _userService = userService;
    private readonly IMapper _mapper = mapper;
    private readonly EntryRepository _entryRepository = entryRepository;

    public async Task CreateEntry(CreateEntryRequest request, ClaimsPrincipal user)
    {
        request.Unbound.CreatedById = _userService.GetUserId(user)
            ?? throw new Exception("UserId not found");

        var entity = _mapper.Map<Entry>(request);

        entity.Selections.ForEach(s =>
        {
            s.CreatedById = entity.CreatedById;
            s.State = entity.State;
        });

        var result = await _entryRepository.CreateEntry(entity);
    }
}