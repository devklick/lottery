
using Lottery.Api.Models.EntrySelection.Create;

namespace Lottery.Api.Mappings;

public class EntrySelectionMappingProfile : AutoMapper.Profile
{
    public EntrySelectionMappingProfile()
    {
        MapModelsForCreate();
    }

    private void MapModelsForCreate()
    {
        CreateMap<CreateEntrySelectionRequestBody, DB.Entities.Dbo.EntrySelection>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}