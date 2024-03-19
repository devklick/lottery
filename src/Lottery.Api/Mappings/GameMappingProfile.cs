using Lottery.Api.Models.Game.Create;
using Lottery.Api.Models.Game.Search;


namespace Lottery.Api.Mappings;

public class GameMappingProfile : AutoMapper.Profile
{
    public GameMappingProfile()
    {
        MapModelsForCreate();
        MapModelsForGet();
    }

    private void MapModelsForCreate()
    {
        CreateMap<CreateGameRequest, DB.Entities.Dbo.Game>()
            .IncludeMembers(src => src.Body, src => src.Unbound);

        CreateMap<CreateGameRequestBody, DB.Entities.Dbo.Game>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<CreateGameRequestUnbound, DB.Entities.Dbo.Game>();

        CreateMap<DB.Entities.Dbo.Game, CreateGameResponse>();
    }

    private void MapModelsForGet()
    {
        CreateMap<DB.Entities.Dbo.Game, SearchGamesResponseItem>();
    }
}