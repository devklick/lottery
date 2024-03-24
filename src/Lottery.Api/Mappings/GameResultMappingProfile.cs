using Lottery.Api.Models.GameResult.Get;
using Lottery.Api.Models.GameResult.Search;

namespace Lottery.Api.Mappings;

public class GameResultMappingProfile : AutoMapper.Profile
{
    public GameResultMappingProfile()
    {
        MapModelsForGet();
    }

    private void MapModelsForGet()
    {
        CreateMap<DB.Entities.Dbo.GameResult, SearchGameResultResponseItem>()
            .IncludeMembers(src => src.Selection);
        CreateMap<DB.Entities.Dbo.GameResult, GetGameResultResponse>()
            .IncludeMembers(src => src.Selection);
    }
}