using Lottery.Api.Models.Account.SignIn;
using Lottery.Repository.Entities.Idt;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(ILogger<AccountController> logger, SignInManager<AppUser> signInManager) : ControllerBase
{
    private readonly ILogger<AccountController> _logger = logger;
    private readonly SignInManager<AppUser> _signInManager = signInManager;


    [HttpPost("signIn")]
    public async Task<ActionResult<SignInResponseBody>> SignIn(SignInRequest request)
    {
        var result = await _signInManager.PasswordSignInAsync(
            request.Body.Username, request.Body.Password, request.Body.StaySignedIn, true);

        return result.Succeeded ? Ok() : Unauthorized();
    }
}
