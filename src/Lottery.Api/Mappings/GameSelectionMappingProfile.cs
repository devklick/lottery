using Lottery.Api.Models.GameSelection.Create;
using Lottery.Api.Models.GameSelection.Get;
using Lottery.Api.Models.GameSelection.Search;

namespace Lottery.Api.Mappings;

public class GameSelectionMappingProfile : AutoMapper.Profile
{
    public GameSelectionMappingProfile()
    {
        MapModelsForCreate();
        MapModelsForGet();
    }

    private void MapModelsForCreate()
    {
        CreateMap<CreateGameSelectionRequestBody, DB.Entities.Dbo.GameSelection>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<DB.Entities.Dbo.GameSelection, CreateGameSelectionResponse>();
    }

    private void MapModelsForGet()
    {
        CreateMap<DB.Entities.Dbo.GameSelection, SearchGameSelectionResponseItem>();
        CreateMap<DB.Entities.Dbo.GameSelection, GetGameSelectionResponse>();
    }
}