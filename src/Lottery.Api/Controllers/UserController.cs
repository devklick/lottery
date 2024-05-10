using Lottery.Api.Models.User.Invite;
using Lottery.Api.Services;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "GameAdmin,SystemAdmin")]
public class UserController(UserService userService) : ApiControllerBase
{
    private readonly UserService _userService = userService;


    [HttpPost("invite")]
    public async Task<ActionResult<UserInviteResponse>> Invite(UserInviteRequest request)
    {
        var result = await _userService.InviteUser(request, User);

        return CreateActionResult(result);
    }
}