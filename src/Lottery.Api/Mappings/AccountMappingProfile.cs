using Lottery.Api.Models.Account.SignUp;
using Lottery.DB.Entities.Idt;

namespace Lottery.Api.Mappings;

public class AccountMappingProfile : AutoMapper.Profile
{
    public AccountMappingProfile()
    {
        CreateMap<SignUpRequest, AppUser>()
            .IncludeMembers(src => src.Body);

        CreateMap<SignUpRequestBody, AppUser>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<AppUser, SignUpResponse>();
    }
}