using Lottery.Api.Filters.Auth;
using Lottery.Api.Mappings;
using Lottery.Api.Models.Account.ConfirmEmail;
using Lottery.Api.Models.Account.ConfirmEmailChange;
using Lottery.Api.Models.Account.ConfirmPassword;
using Lottery.Api.Models.Account.DeleteAccount;
using Lottery.Api.Models.Account.ForgotPassword;
using Lottery.Api.Models.Account.GetAccount;
using Lottery.Api.Models.Account.ResetPassword;
using Lottery.Api.Models.Account.SignIn;
using Lottery.Api.Models.Account.SignUp;
using Lottery.Api.Models.Account.UpdateAccount;
using Lottery.Api.Models.Account.UpdatePassword;
using Lottery.Api.Services;
using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(
    SignInManager<AppUser> signInManager,
    UserService userService,
    ResultMapper resultMapper)
    : ApiControllerBase(resultMapper)
{

    [HttpPost("signIn")]
    public async Task<ActionResult<SignInResponse>> SignIn(SignInRequest request)
    {
        var result = await userService.SignIn(request);

        return CreateObjectResult(result);
    }

    [HttpPost("signOut")]
    public new async Task<ActionResult> SignOut()
    {
        await signInManager.SignOutAsync();
        return base.SignOut();
    }

    [HttpPost("signUp")]
    public async Task<ActionResult<SignUpResponse>> SignUp(SignUpRequest request)
    {
        var result = await userService.CreateAccount(request);

        return CreateObjectResult(result);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<GetAccountResponse>> GetAccount()
    {
        var result = await userService.GetAccount();

        return CreateObjectResult(result);
    }

    [Authorize, RequireRecentAuth]
    [HttpPost]
    public async Task<ActionResult<UpdateAccountResponse>> UpdateAccount(UpdateAccountRequest request)
    {
        var result = await userService.UpdateAccount(request);

        return CreateObjectResult(result);
    }

    [Authorize]
    [HttpPost("password/confirm")]
    public async Task<ActionResult<ConfirmPasswordResponse>> ConfirmPassword(ConfirmPasswordRequest request)
    {
        var result = await userService.ConfirmPassword(request);

        return CreateObjectResult(result);
    }

    [Authorize]
    [HttpPost("password/update")]
    public async Task<ActionResult<UpdatePasswordResponse>> UpdatePassword(UpdatePasswordRequest request)
    {
        var result = await userService.UpdatePassword(request);

        return CreateObjectResult(result);
    }

    [HttpPost("password/forgot")]
    public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword(ForgotPasswordRequest request)
    {
        var result = await userService.ForgotPassword(request);

        return CreateObjectResult(result);
    }

    [HttpPost("password/reset")]
    public async Task<ActionResult<ResetPasswordResponse>> ResetPassword(ResetPasswordRequest request)
    {
        var result = await userService.ResetPassword(request);

        return CreateObjectResult(result);
    }

    [HttpGet("email/confirm")]
    public async Task<ActionResult<ConfirmEmailResponse>> ConfirmEmail(ConfirmEmailRequest request)
    {
        var result = await userService.ConfirmEmail(request);

        return CreateObjectResult(result);
    }

    [HttpGet("email/confirmChange")]
    public async Task<ActionResult<ConfirmEmailChangeResponse>> ConfirmEmailChange(ConfirmEmailChangeRequest request)
    {
        var result = await userService.ConfirmEmailChange(request);

        return CreateObjectResult(result);
    }

    [HttpDelete]
    [Authorize, RequireRecentAuth]
    public async Task<ActionResult<DeleteAccountResponse>> Delete()
    {
        var result = await userService.DeleteAccount();

        return CreateObjectResult(result);
    }
}
