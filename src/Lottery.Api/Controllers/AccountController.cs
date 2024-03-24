using Lottery.Api.Models.Account.SignIn;
using Lottery.Api.Models.Account.SignUp;
using Lottery.Api.Services;
using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(
    SignInManager<AppUser> signInManager,
    UserService userService) : ApiControllerBase
{
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly UserService _userService = userService;

    [HttpPost("signIn")]
    public async Task<ActionResult<SignInResponse>> SignIn(SignInRequest request)
    {
        var result = await _userService.SignIn(request);

        return CreateActionResult(result);
    }

    [HttpPost("signOut")]
    public new async Task<ActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return base.SignOut();
    }

    [HttpPost("signUp")]
    public async Task<ActionResult<SignUpResponse>> SignUp(SignUpRequest request)
    {
        var result = await _userService.CreateAccount(request);

        return CreateActionResult(result);
    }
}
