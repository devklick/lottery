using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Models.Account.SignIn;
using Lottery.Api.Models.Account.SignUp;
using Lottery.Api.Models.Common;
using Lottery.Api.Services.Options;
using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Lottery.Api.Services;

public class UserService(
    UserManager<AppUser> userManager,
    IMapper mapper,
    IOptions<UserServiceOptions> userServiceOptions,
    IOptions<CookieAuthenticationOptions> cookieOptions,
    PasswordHasher<AppUser> passwordHasher,
    SignInManager<AppUser> signInManager)
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IMapper _mapper = mapper;
    private readonly IOptions<UserServiceOptions> _userServiceOptions = userServiceOptions;
    private readonly IOptions<CookieAuthenticationOptions> _cookieOptions = cookieOptions;
    private readonly PasswordHasher<AppUser> _passwordHasher = passwordHasher;
    private readonly SignInManager<AppUser> _signInManager = signInManager;

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

    public async Task<Result<SignInResponse>> SignIn(SignInRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Body.Username);

        if (user == null)
        {
            return new Result<SignInResponse>
            {
                Status = ResultStatus.NotFound,
                Errors = [new() { Message = "User name not registered" }]
            };
        }

        var result = await _signInManager.PasswordSignInAsync(request.Body.Username, request.Body.Password, request.Body.StaySignedIn, true);

        if (!result.Succeeded)
        {
            return new Result<SignInResponse>
            {
                Status = ResultStatus.NotAuthenticated,
                Errors = [new() { Message = "Invalid sign in credentials" }]
            };
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, "GameAdmin");

        return new Result<SignInResponse>
        {
            Status = ResultStatus.Ok,
            Value = new SignInResponse
            {
                SessionExpiry = DateTime.UtcNow.Add(_cookieOptions.Value.ExpireTimeSpan),
                UserType = isAdmin ? UserType.Admin : UserType.Basic
            }
        };
    }

    public async Task<Result<SignUpResponse>> CreateAccount(SignUpRequest request)
    {
        var appUser = _mapper.Map<AppUser>(request);

        appUser.EmailConfirmed = _userServiceOptions.Value.AutoConfirmNewAccounts;
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