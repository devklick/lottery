using Lottery.Api.Models.GamePrize.Create;
using Lottery.Api.Models.GamePrize.Get;
using Lottery.Api.Models.GameSelection.Search;

namespace Lottery.Api.Mappings;

public class GamePrizeMappingProfile : AutoMapper.Profile
{
    public GamePrizeMappingProfile()
    {
        MapModelsForCreate();
        MapModelsForGet();
    }

    private void MapModelsForCreate()
    {
        CreateMap<CreateGamePrizeRequestBody, DB.Entities.Dbo.GamePrize>();
        CreateMap<DB.Entities.Dbo.GamePrize, CreateGamePrizeResponse>();
    }

    private void MapModelsForGet()
    {
        CreateMap<DB.Entities.Dbo.GamePrize, SearchGamePrizeResponseItem>();
        CreateMap<DB.Entities.Dbo.GamePrize, GetGamePrizeResponse>();
    }
}