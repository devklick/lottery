using Lottery.Api.Models.Account.SignIn;
using Lottery.Api.Models.Account.SignUp;
using Lottery.Api.Services;
using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(ILogger<AccountController> logger, SignInManager<AppUser> signInManager, UserService userService) : ApiControllerBase
{
    private readonly ILogger<AccountController> _logger = logger;
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly UserService _userService = userService;

    [HttpPost("signIn")]
    public async Task<ActionResult<SignInResponse>> SignIn(SignInRequest request)
    {
        if (User != null && _signInManager.IsSignedIn(User))
        {
            return Ok();
        }

        var result = await _signInManager.PasswordSignInAsync(
            request.Body.Username, request.Body.Password, request.Body.StaySignedIn, true);

        return result.Succeeded ? Ok() : Unauthorized();
    }

    [HttpPost("signUp")]
    public async Task<ActionResult<SignUpResponse>> SignUp(SignUpRequest request)
    {
        var result = await _userService.CreateAccount(request);

        return CreateActionResult(result);
    }
}
