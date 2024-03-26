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

        CreateMap<CreateGameRequestBody.Prize, DB.Entities.Dbo.GamePrize>();

        CreateMap<CreateGameRequestUnbound, DB.Entities.Dbo.Game>();

        CreateMap<DB.Entities.Dbo.Game, CreateGameResponse>();
        CreateMap<DB.Entities.Dbo.GamePrize, CreateGameResponse.Prize>();
        CreateMap<DB.Entities.Dbo.GameSelection, CreateGameResponse.Selection>();
    }

    private void MapModelsForGet()
    {
        CreateMap<DB.Entities.Dbo.Game, SearchGamesResponseItem>();
        CreateMap<DB.Entities.Dbo.Game, GetGameResponse>();
        CreateMap<DB.Entities.Dbo.GameSelection, GetGameResponse.Selection>();
        CreateMap<DB.Entities.Dbo.GameResult, GetGameResponse.Result>()
            .IncludeMembers(src => src.Selection);
        CreateMap<DB.Entities.Dbo.GameSelection, GetGameResponse.Result>();
        CreateMap<DB.Entities.Dbo.GamePrize, GetGameResponse.Prize>();
        CreateMap<DB.Entities.Dbo.GamePrize, SearchGamesResponseItem.Prize>();
        CreateMap<DB.Entities.Dbo.GameResult, SearchGamesResponseItem.Result>()
            .IncludeMembers(src => src.Selection);
        CreateMap<DB.Entities.Dbo.GameSelection, SearchGamesResponseItem.Selection>();
        CreateMap<DB.Entities.Dbo.GameSelection, SearchGamesResponseItem.Result>();
    }

    private void MapModelsForEdit()
    {
        CreateMap<EditGameRequestBody, DB.Entities.Dbo.Game>();
        CreateMap<DB.Entities.Dbo.Game, EditGameResponse>();
        CreateMap<DB.Entities.Dbo.GameResult, EditGameResponse.Result>();
        CreateMap<DB.Entities.Dbo.GamePrize, EditGameResponse.Prize>();
        CreateMap<DB.Entities.Dbo.GameSelection, EditGameResponse.Selection>();
    }
}