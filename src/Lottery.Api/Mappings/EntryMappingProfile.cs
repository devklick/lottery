
using Lottery.Api.Models.Entry.Create;
using Lottery.Api.Models.Entry.Search;

namespace Lottery.Api.Mappings;

public class EntryMappingProfile : AutoMapper.Profile
{
    public EntryMappingProfile()
    {
        MapModelsForCreate();
        MapModelsForGet();
    }

    private void MapModelsForCreate()
    {
        CreateMap<CreateEntryRequest, DB.Entities.Dbo.Entry>()
            .IncludeMembers(src => src.Body, src => src.Unbound);

        CreateMap<CreateEntryRequestBody, DB.Entities.Dbo.Entry>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<CreateEntryRequestUnbound, DB.Entities.Dbo.Entry>();

        CreateMap<CreateEntryRequestBody.Selection, DB.Entities.Dbo.EntrySelection>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }

    private void MapModelsForGet()
    {
        CreateMap<DB.Entities.Dbo.Entry, SearchEntriesResponseItem>();

        CreateMap<DB.Entities.Dbo.EntrySelection, SearchEntriesResponseItem.Selection>()
            .IncludeMembers(src => src.GameSelection);
        CreateMap<DB.Entities.Dbo.GameSelection, SearchEntriesResponseItem.Selection>();
        CreateMap<DB.Entities.Dbo.EntryPrize, SearchEntriesResponseItem.EntryPrize>();

    }

}