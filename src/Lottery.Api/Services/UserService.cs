using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Models.Account.SignUp;
using Lottery.Api.Models.Common;
using Lottery.Api.Services.Options;
using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Lottery.Api.Services;

public class UserService(UserManager<AppUser> userManager, IMapper mapper, IOptions<UserServiceOptions> options, PasswordHasher<AppUser> passwordHasher)
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IMapper _mapper = mapper;
    private readonly IOptions<UserServiceOptions> _options = options;
    private readonly PasswordHasher<AppUser> _passwordHasher = passwordHasher;

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
        appUser.PasswordHash = _passwordHasher.HashPassword(appUser, request.Body.Password);

        var userResult = await _userManager.CreateAsync(appUser);

        if (!userResult.Succeeded)
        {
            return new Result<SignUpResponse>
            {
                Status = ResultStatus.ServerError,
                Errors = userResult.Errors.Select(s => new Error { Message = s.Description }).ToList()
            };
        }

        var roleResult = await _userManager.AddToRoleAsync(appUser, "BasicUser");


        return roleResult.Succeeded ? new Result<SignUpResponse>
        {
            Status = ResultStatus.Ok,
        } : new Result<SignUpResponse>
        {
            Status = ResultStatus.ServerError,
            Errors = roleResult.Errors.Select(s => new Error { Message = s.Description }).ToList()
        };
    }
}