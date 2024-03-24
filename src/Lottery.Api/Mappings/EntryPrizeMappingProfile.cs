using Lottery.Api.Models.EntryPrize.Search;

namespace Lottery.Api.Mappings;

public class EntryPrizeMappingProfile : AutoMapper.Profile
{
    public EntryPrizeMappingProfile()
    {
        MapModelsForGet();
    }

    private void MapModelsForGet()
    {
        CreateMap<DB.Entities.Dbo.EntryPrize, SearchEntryPrizeResponseItem>();
    }
}