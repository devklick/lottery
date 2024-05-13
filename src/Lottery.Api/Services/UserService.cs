using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Models.Account.SignIn;
using Lottery.Api.Models.Account.SignUp;
using Lottery.Api.Models.Common;
using Lottery.Api.Models.User.Invite;
using Lottery.Api.Models.User.Invite.Accept;
using Lottery.Api.Models.User.Invite.Verify;
using Lottery.Api.Repositories.User;
using Lottery.Api.Services.Options;
using Lottery.Api.Utilities;
using Lottery.Common.Models;
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
    SignInManager<AppUser> signInManager,
    Hasher hasher,
    UserRepository userRepository)
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IMapper _mapper = mapper;
    private readonly IOptions<UserServiceOptions> _userServiceOptions = userServiceOptions;
    private readonly IOptions<CookieAuthenticationOptions> _cookieOptions = cookieOptions;
    private readonly Hasher _hasher = hasher;
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly UserRepository _userRepository = userRepository;

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

    public async Task<Result<SignUpResponse>> CreateAccount(SignUpRequest request, IEnumerable<string>? roles = null)
    {
        var appUser = _mapper.Map<AppUser>(request);

        appUser.EmailConfirmed = _userServiceOptions.Value.AutoConfirmNewAccounts;
        appUser.PasswordHash = _hasher.HashPassword(appUser, request.Body.Password);

        var userResult = await _userManager.CreateAsync(appUser);

        if (!userResult.Succeeded)
        {
            return new Result<SignUpResponse>
            {
                Status = ResultStatus.ServerError,
                Errors = userResult.Errors.Select(s => new Error { Message = s.Description }).ToList()
            };
        }

        if (roles == null || !roles.Any())
        {
            roles = ["BasicUser"];
        }

        foreach (var role in roles)
        {
            var roleResult = await _userManager.AddToRoleAsync(appUser, role);

            if (!roleResult.Succeeded)
            {
                return new Result<SignUpResponse>
                {
                    Status = ResultStatus.ServerError,
                    Errors = roleResult.Errors.Select(s => new Error { Message = s.Description }).ToList()
                };
            }
        }


        return new Result<SignUpResponse> { Status = ResultStatus.Ok };
    }

    public async Task<Result<UserInviteResponse>> InviteUser(UserInviteRequest request, ClaimsPrincipal user)
    {
        // Get the ID of the user who is inviting another user
        var userIdResult = GetUserId(user);
        if (userIdResult.Status != ResultStatus.Ok)
        {
            return new Result<UserInviteResponse>
            {
                Status = userIdResult.Status,
                Errors = userIdResult.Errors
            };
        }

        // Get the role which the user is being invited to
        var role = await _userRepository.GetRole(request.Body.UserType);
        if (role == null)
        {
            return new Result<UserInviteResponse>
            {
                Status = ResultStatus.ServerError,
                Errors = [new() { Message = $"Unable to find role for user type {request.Body.UserType}" }]
            };
        }

        var invite = new AppUserInvite
        {
            Id = Guid.NewGuid(),
            Email = request.Body.Email,
            Token = _hasher.GetString(128),
            CreatedById = userIdResult.Value,
            Expiry = DateTime.UtcNow.Add(_userServiceOptions.Value.UserInviteValidFor),
        };

        // Create the invite
        var inviteResult = await _userRepository.CreateUserInvite(invite);

        var inviteRole = new AppUserInviteRole
        {
            AppUserInviteId = inviteResult.Id,
            AppRoleId = role.Id
        };

        // Link the invite to the role that the user is being invited to
        await _userRepository.CreateUserInviteRoles(inviteRole);

        await _userRepository.SaveChangesAsync();

        // Send email to invite user - not going to implement this

        return new Result<UserInviteResponse>
        {
            Status = ResultStatus.Ok,
            Value = new UserInviteResponse
            {
                Email = invite.Email,
                Token = invite.Token
            }
        };
    }

    public async Task<Result<VerifyUserInviteResponse>> FindUserInvite(VerifyUserInviteRequest request)
    {
        var invite = await _userRepository.FindUserInvite(request.Query.Email, request.Query.Token);

        if (invite == null)
        {
            return new Result<VerifyUserInviteResponse> { Status = ResultStatus.NotFound };
        }

        if (invite.Expiry < DateTime.UtcNow)
        {
            return new Result<VerifyUserInviteResponse>
            {
                Status = ResultStatus.BadRequest,
                Errors = [new() { Message = "Invite Expired" }]
            };
        }

        if (invite.AppUserId != null)
        {
            return new Result<VerifyUserInviteResponse>
            {
                Status = ResultStatus.BadRequest,
                Errors = [new() { Message = "Invite already accepted" }]
            };
        }

        return new Result<VerifyUserInviteResponse> { Status = ResultStatus.Ok };
    }

    public async Task<Result<AcceptUserInviteResponse>> AcceptUserInvite(AcceptUserInviteRequest request)
    {
        var invite = await _userRepository.FindUserInvite(request.Body.Email, request.Body.Token,
            rolesFilter: new()
            {
                Include = true
            }
        );

        if (invite == null)
        {
            return new Result<AcceptUserInviteResponse> { Status = ResultStatus.NotFound };
        }

        if (invite.Expiry < DateTime.UtcNow)
        {
            return new Result<AcceptUserInviteResponse>
            {
                Status = ResultStatus.BadRequest,
                Errors = [new() { Message = "Invite Expired" }]
            };
        }

        if (invite.AppUserId != null)
        {
            return new Result<AcceptUserInviteResponse>
            {
                Status = ResultStatus.BadRequest,
                Errors = [new() { Message = "Invite already accepted" }]
            };
        }

        var roles = invite.AppUserInviteRoles.Select(r => r.AppRole.Name ?? "").ToList();

        var accountResult = await CreateAccount(new SignUpRequest
        {
            Body = new SignUpRequestBody
            {
                Email = request.Body.Email,
                Password = request.Body.Password,
                Username = request.Body.Username
            }
        }, roles);

        return new Result<AcceptUserInviteResponse>
        {
            Status = accountResult.Status,
            Errors = accountResult.Errors,
        };
    }
}