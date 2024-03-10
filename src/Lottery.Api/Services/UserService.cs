using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Models.Account.SignUp;
using Lottery.Api.Models.Common;
using Lottery.Api.Services.Options;
using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Lottery.Api.Services;

public class UserService(UserManager<AppUser> userManager, IMapper mapper, IOptions<UserServiceOptions> options)
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IMapper _mapper = mapper;
    private readonly IOptions<UserServiceOptions> _options = options;

    public Result<Guid> GetUserId(ClaimsPrincipal user)
    {
        var id = _userManager.GetUserId(user);

        if (id == null)
        {
            return new Result<Guid>
            {
                Status = ResultStatus.NotAuthenticated,
                Errors = [new() { Message = "Unable to locate user" }]
            };
        }

        return new Result<Guid>
        {
            Status = ResultStatus.Ok,
            Value = Guid.Parse(id),
        };
    }

    public async Task<Result<SignUpResponse>> CreateAccount(SignUpRequest request)
    {
        var appUser = _mapper.Map<AppUser>(request);

        appUser.EmailConfirmed = _options.Value.AutoConfirmNewAccounts;

        var result = await _userManager.CreateAsync(appUser);

        return result.Succeeded ? new Result<SignUpResponse>
        {
            Status = ResultStatus.Ok,
        } : new Result<SignUpResponse>
        {
            Status = ResultStatus.ServerError,
            Errors = result.Errors.Select(s => new Error { Message = s.Description }).ToList()
        };
    }
}