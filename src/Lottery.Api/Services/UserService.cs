using System.Security.Claims;

using AutoMapper;

using Lottery.Api.Models.Account.ConfirmEmail;
using Lottery.Api.Models.Account.ConfirmEmailChange;
using Lottery.Api.Models.Account.ConfirmPassword;
using Lottery.Api.Models.Account.GetAccount;
using Lottery.Api.Models.Account.SignIn;
using Lottery.Api.Models.Account.SignUp;
using Lottery.Api.Models.Account.UpdateAccount;
using Lottery.Api.Models.Account.UpdatePassword;
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
    UserRepository userRepository,
    TimeProvider timeProvider,
    IHttpContextAccessor httpContextAccessor,
    IEmailSender<AppUser> emailSender)
{

    public Result<Guid> GetUserId(ClaimsPrincipal user)
    {
        var id = userManager.GetUserId(user);

        if (id == null)
        {
            return new Result<Guid>
            {
                Status = ResultStatus.NotAuthenticated,
                Messages = [new() { Value = "Unable to locate user" }]
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
        var user = await userManager.FindByNameAsync(request.Body.Username);

        if (user == null)
        {
            return new Result<SignInResponse>
            {
                Status = ResultStatus.NotFound,
                Messages = [new() { Value = "Username not registered" }]
            };
        }

        var result = await signInManager.PasswordSignInAsync(request.Body.Username, request.Body.Password, request.Body.StaySignedIn, true);

        if (!result.Succeeded)
        {
            return new Result<SignInResponse>
            {
                Status = ResultStatus.NotAuthenticated,
                Messages = [new() { Value = "Invalid sign in credentials" }]
            };
        }

        var isAdmin = await userManager.IsInRoleAsync(user, "GameAdmin");

        return new Result<SignInResponse>
        {
            Status = ResultStatus.Ok,
            Value = new SignInResponse
            {
                SessionExpiry = timeProvider.GetUtcNow().Add(cookieOptions.Value.ExpireTimeSpan).DateTime,
                UserType = isAdmin ? UserType.Admin : UserType.Basic
            }
        };
    }

    public async Task<Result<SignUpResponse>> CreateAccount(SignUpRequest request, IEnumerable<string>? roles = null)
    {
        var appUser = mapper.Map<AppUser>(request);

        var userResult = await userManager.CreateAsync(appUser);

        if (!userResult.Succeeded)
        {
            return Result<SignUpResponse>.Error(
                ResultStatus.ServerError,
                userResult.Errors.Select(s => new Message { Value = s.Description }));
        }

        appUser.EmailConfirmed = userServiceOptions.Value.AutoConfirmNewAccounts;
        appUser.PasswordHash = hasher.HashPassword(appUser, request.Body.Password);

        if (roles == null || !roles.Any())
        {
            roles = ["BasicUser"];
        }

        foreach (var role in roles)
        {
            var roleResult = await userManager.AddToRoleAsync(appUser, role);

            if (!roleResult.Succeeded)
            {
                return Result<SignUpResponse>.Error(
                    ResultStatus.ServerError,
                    roleResult.Errors.Select(s => new Message { Value = s.Description, Code = MessageCode.General }));
            }
        }

        var user = await userManager.FindByEmailAsync(appUser.Email!);
        if (user is null)
        {
            return Result<SignUpResponse>.Error(
                ResultStatus.ServerError,
                "Unexpected error when creating user");
        }

        var changeEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

        var confirmationLink =
            $"{userServiceOptions.Value.EmailConfirmationDomain}/account/confirmEmail" +
            $"?userId={user.Id}" +
            $"&token={Uri.EscapeDataString(changeEmailToken)}";

        await emailSender.SendConfirmationLinkAsync(user, request.Body.Email, confirmationLink);

        return Result<SignUpResponse>.Ok(new());
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
                Messages = userIdResult.Messages
            };
        }

        // Get the role which the user is being invited to
        var role = await userRepository.GetRole(request.Body.UserType);
        if (role == null)
        {
            return new Result<UserInviteResponse>
            {
                Status = ResultStatus.ServerError,
                Messages = [new() { Value = $"Unable to find role for user type {request.Body.UserType}" }]
            };
        }

        var invite = new AppUserInvite
        {
            Id = Guid.NewGuid(),
            Email = request.Body.Email,
            Token = hasher.GetString(128),
            CreatedById = userIdResult.Value,
            Expiry = timeProvider.GetUtcNow().Add(userServiceOptions.Value.UserInviteValidFor).DateTime,
        };

        // Create the invite
        var inviteResult = await userRepository.CreateUserInvite(invite);

        var inviteRole = new AppUserInviteRole
        {
            AppUserInviteId = inviteResult.Id,
            AppRoleId = role.Id
        };

        // Link the invite to the role that the user is being invited to
        await userRepository.CreateUserInviteRoles(inviteRole);

        await userRepository.SaveChangesAsync();

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
        var invite = await userRepository.FindUserInvite(request.Query.Email, request.Query.Token);

        if (invite == null)
        {
            return new Result<VerifyUserInviteResponse> { Status = ResultStatus.NotFound };
        }

        if (invite.Expiry < timeProvider.GetUtcNow())
        {
            return new Result<VerifyUserInviteResponse>
            {
                Status = ResultStatus.BadRequest,
                Messages = [new() { Value = "Invite Expired" }]
            };
        }

        if (invite.AppUserId != null)
        {
            return new Result<VerifyUserInviteResponse>
            {
                Status = ResultStatus.BadRequest,
                Messages = [new() { Value = "Invite already accepted" }]
            };
        }

        return new Result<VerifyUserInviteResponse> { Status = ResultStatus.Ok };
    }

    public async Task<Result<AcceptUserInviteResponse>> AcceptUserInvite(AcceptUserInviteRequest request)
    {
        var invite = await userRepository.FindUserInvite(request.Body.Email, request.Body.Token,
            rolesFilter: new()
            {
                Include = true
            }
        );

        if (invite == null)
        {
            return new Result<AcceptUserInviteResponse> { Status = ResultStatus.NotFound };
        }

        if (invite.Expiry < timeProvider.GetUtcNow())
        {
            return new Result<AcceptUserInviteResponse>
            {
                Status = ResultStatus.BadRequest,
                Messages = [new() { Value = "Invite Expired" }]
            };
        }

        if (invite.AppUserId != null)
        {
            return new Result<AcceptUserInviteResponse>
            {
                Status = ResultStatus.BadRequest,
                Messages = [new() { Value = "Invite already accepted" }]
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
            Messages = accountResult.Messages,
        };
    }

    public async Task<Result<GetAccountResponse>> GetAccount()
    {
        var userResult = await GetCurrentUser();

        return userResult.ChangeValue<GetAccountResponse>(userResult.Success ? new()
        {
            Email = userResult.Value.Email!,
            EmailConfirmed = userResult.Value.EmailConfirmed,
            PhoneNumber = userResult.Value.PhoneNumber,
            PhoneNumberConfirmed = userResult.Value.PhoneNumberConfirmed,
            Username = userResult.Value.UserName!
        } : null);
    }

    public async Task<Result<UpdateAccountResponse>> UpdateAccount(UpdateAccountRequest request)
    {
        var userResult = await GetCurrentUser();
        if (!userResult.Success) return userResult.ChangeValue<UpdateAccountResponse>();

        var user = userResult.Value;

        var updateResult = new UpdateAccountResponse
        {
            Email = Result<string>.Ok(user.Email!),
            Username = Result<string>.Ok(user.UserName!),
            PhoneNumber = Result<string?>.Ok(user.PhoneNumber)
        };

        // TODO: Consider storing the email that the user has requested to change to. 
        // Useful to show in UI to indicate that the account is going through a change-email process. 
        // Without this, nothing in the UI indicates that there's a change email pending.
        if (request.Body.Username is not null && request.Body.Username != user.UserName)
        {
            var usernameResult = await UpdateUsername(user, request.Body.Username);

            if (!usernameResult.Success)
                updateResult.Username = usernameResult.ChangeValue(user.UserName);
            else
            {
                user = usernameResult.Value;
                updateResult.Username = usernameResult.ChangeValue(user.UserName);
            }
        }

        if (request.Body.Email is not null && request.Body.Email != user.Email)
        {
            var changeEmailToken = await userManager.GenerateChangeEmailTokenAsync(user, request.Body.Email);
            var confirmationLink =
                $"{userServiceOptions.Value.EmailConfirmationDomain}/account/confirmEmailChange" +
                $"?userId={user.Id}" +
                $"&email={Uri.EscapeDataString(request.Body.Email)}" +
                $"&token={Uri.EscapeDataString(changeEmailToken)}";

            await emailSender.SendConfirmationLinkAsync(user, request.Body.Email, confirmationLink);

            updateResult.Email = Result<string>.Ok(request.Body.Email);
            updateResult.Email.AddMessages(MessageCode.EmailVerificationRequired, "Email change pending confirmation");
        }

        if (request.Body.PhoneNumber is not null && request.Body.PhoneNumber != user.PhoneNumber)
        {
            // TODO: Consider proper flow for changing phone number
            await userManager.SetPhoneNumberAsync(user, request.Body.PhoneNumber);
            updateResult.PhoneNumber.ChangeValue(request.Body.PhoneNumber);
        }

        return Result<UpdateAccountResponse>.Ok(updateResult);
    }

    public async Task<Result<AppUser>> GetCurrentUser(string? username = null)
    {
        username ??= httpContextAccessor.HttpContext?.User.Identity?.Name;
        if (username is null)
        {
            return Result<AppUser>.Error(ResultStatus.NotAuthenticated, "User not logged in");
        }

        var user = await userManager.FindByNameAsync(username);

        if (user is null)
        {
            return Result<AppUser>.Error(ResultStatus.NotFound, "User not found");
        }
        return Result<AppUser>.Ok(user);
    }

    public async Task<bool> HasRecentAuth(AppUser user)
    {
        return user.LastReauthenticatedAt.HasValue
            && user.LastReauthenticatedAt > timeProvider
            .GetUtcNow()
            .Add(-userServiceOptions.Value.ElevatedSessionDuration);
    }

    private async Task<Result<AppUser>> UpdateUsername(AppUser user, string username)
    {
        var result = await userManager.SetUserNameAsync(user, username);
        return result.Succeeded
            ? await GetCurrentUser(username)
            : Result<AppUser>.Error(ResultStatus.BadRequest, result.Errors.Select(s => s.Description));
    }

    public async Task<Result<ConfirmPasswordResponse>> ConfirmPassword(ConfirmPasswordRequest request)
    {
        var userResult = await GetCurrentUser();
        if (!userResult.Success) return userResult.ChangeValue<ConfirmPasswordResponse>();
        var user = userResult.Value;
        var valid = await userManager.CheckPasswordAsync(user, request.Body.Password);
        if (valid)
        {
            user.LastReauthenticatedAt = timeProvider.GetUtcNow().UtcDateTime;
            await userRepository.UpdateUser(user);
            return Result<ConfirmPasswordResponse>.Ok(new());
        }
        return Result<ConfirmPasswordResponse>.Error(ResultStatus.NotAuthenticated, "Incorrect password");
    }

    public async Task<Result<ConfirmEmailResponse>> ConfirmEmail(ConfirmEmailRequest request)
    {
        var user = await userManager.FindByIdAsync(request.Query.UserId.ToString());
        if (user is null)
        {
            return Result<ConfirmEmailResponse>.Error(
                ResultStatus.NotFound,
                "Unable to locate user"
            );
        }
        var result = await userManager.ConfirmEmailAsync(user, request.Query.Token);

        return result.Succeeded
            ? Result<ConfirmEmailResponse>.Ok(new())
            : Result<ConfirmEmailResponse>.Error(ResultStatus.ServerError, result.Errors.Select(e => e.Description));
    }

    public async Task<Result<ConfirmEmailChangeResponse>> ConfirmEmailChange(ConfirmEmailChangeRequest request)
    {
        var user = await userManager.FindByIdAsync(request.Query.UserId.ToString());
        if (user is null)
        {
            return Result<ConfirmEmailChangeResponse>.Error(
                ResultStatus.NotFound,
                "Unable to locate user"
            );
        }

        var result = await userManager.ChangeEmailAsync(user, Uri.UnescapeDataString(request.Query.Email), Uri.UnescapeDataString(request.Query.Token));

        return result.Succeeded
            ? Result<ConfirmEmailChangeResponse>.Ok(new())
            : Result<ConfirmEmailChangeResponse>.Error(ResultStatus.ServerError, result.Errors.Select(e => e.Description));
    }

    public async Task<Result<UpdatePasswordResponse>> UpdatePassword(UpdatePasswordRequest request)
    {
        var userResult = await GetCurrentUser();

        if (!userResult.Success)
        {
            return userResult.ChangeValue<UpdatePasswordResponse>();
        }

        var result = await userManager.ChangePasswordAsync(
            userResult.Value,
            request.Body.CurrentPassword,
            request.Body.NewPassword);

        return result.Succeeded
            ? Result<UpdatePasswordResponse>.Ok(new())
            : Result<UpdatePasswordResponse>.Error(ResultStatus.NotAuthorized, result.Errors.Select(e => e.Description));
    }
}