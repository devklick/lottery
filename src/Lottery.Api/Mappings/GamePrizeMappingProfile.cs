using Lottery.Api.Models.GamePrize.Create;

namespace Lottery.Api.Mappings;

public class GamePrizeMappingProfile : AutoMapper.Profile
{
    public GamePrizeMappingProfile()
    {
        CreateMap<CreateGamePrizeRequestBody, DB.Entities.Dbo.GamePrize>();
        CreateMap<DB.Entities.Dbo.GamePrize, CreateGamePrizeResponseBody>();
    }
}