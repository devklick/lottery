
using Lottery.Api.Models.EntrySelection.Create;
using Lottery.Api.Models.EntrySelection.Search;

namespace Lottery.Api.Mappings;

public class EntrySelectionMappingProfile : AutoMapper.Profile
{
    public EntrySelectionMappingProfile()
    {
        MapModelsForCreate();
        MapModelsForGet();
    }

    private void MapModelsForCreate()
    {
        CreateMap<CreateEntrySelectionRequestBody, DB.Entities.Dbo.EntrySelection>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }

    private void MapModelsForGet()
    {
        CreateMap<DB.Entities.Dbo.EntrySelection, SearchEntrySelectionResponseItem>()
            .IncludeMembers(src => src.GameSelection);
    }
}