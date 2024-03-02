using Lottery.Api.Models.Game.Create;

namespace Lottery.Api.Mappings;

public class GameMappingProfile : AutoMapper.Profile
{
    public GameMappingProfile()
    {
        MapModelsForCreate();
    }

    private void MapModelsForCreate()
    {
        CreateMap<CreateGameRequest, Repository.Entities.Dbo.Game>()
            .IncludeMembers(src => src.Body, src => src.Unbound);

        CreateMap<CreateGameRequestBody, Repository.Entities.Dbo.Game>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<CreateGameRequestUnbound, Repository.Entities.Dbo.Game>();
    }
}