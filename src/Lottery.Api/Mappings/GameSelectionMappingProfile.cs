using Lottery.Api.Models.GameSelection.Create;

namespace Lottery.Api.Mappings;

public class GameSelectionMappingProfile : AutoMapper.Profile
{
    public GameSelectionMappingProfile()
    {
        MapModelsForCreate();
    }

    private void MapModelsForCreate()
    {
        CreateMap<CreateGameSelectionRequestBody, Repository.Entities.Dbo.GameSelection>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}