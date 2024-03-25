using Lottery.Api.Models.Game.Create;
using Lottery.Api.Models.Game.Edit;
using Lottery.Api.Models.Game.Get;
using Lottery.Api.Models.Game.Search;


namespace Lottery.Api.Mappings;

public class GameMappingProfile : AutoMapper.Profile
{
    public GameMappingProfile()
    {
        MapModelsForCreate();
        MapModelsForGet();
        MapModelsForEdit();
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
        CreateMap<DB.Entities.Dbo.Game, GetGameResponse>();
    }

    private void MapModelsForEdit()
    {
        CreateMap<EditGameRequestBody, DB.Entities.Dbo.Game>();
        CreateMap<DB.Entities.Dbo.Game, EditGameResponse>();
        CreateMap<DB.Entities.Dbo.GameResult, EditGameResponse_GameResult>();
        CreateMap<DB.Entities.Dbo.GamePrize, EditGameResponse_GamePrize>();
        CreateMap<DB.Entities.Dbo.GameSelection, EditGameResponse_GameSelection>();
    }
}