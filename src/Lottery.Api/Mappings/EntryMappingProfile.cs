
using Lottery.Api.Models.Entry.Create;

namespace Lottery.Api.Mappings;

public class EntryMappingProfile : AutoMapper.Profile
{
    public EntryMappingProfile()
    {
        MapModelsForCreate();
    }

    private void MapModelsForCreate()
    {
        CreateMap<CreateEntryRequest, DB.Entities.Dbo.Entry>()
            .IncludeMembers(src => src.Body, src => src.Unbound);

        CreateMap<CreateEntryRequestBody, DB.Entities.Dbo.Entry>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<CreateEntryRequestUnbound, DB.Entities.Dbo.Entry>();
    }

}