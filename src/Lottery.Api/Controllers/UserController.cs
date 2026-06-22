using Lottery.Api.Mappings;
using Lottery.Api.Models.User.Invite;
using Lottery.Api.Models.User.Invite.Accept;
using Lottery.Api.Models.User.Invite.Verify;
using Lottery.Api.Services;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "GameAdmin,SystemAdmin")]
public class UserController(
    UserService userService,
    ResultMapper resultMapper)
    : ApiControllerBase(resultMapper)
{

    [HttpPost("invite")]
    public async Task<ActionResult<UserInviteResponse>> Invite(UserInviteRequest request)
    {
        var result = await userService.InviteUser(request, User);

        return CreateObjectResult(result);
    }

    [HttpGet("invite/verify")]
    [AllowAnonymous]
    public async Task<ActionResult<VerifyUserInviteResponse>> VerifyInvite(VerifyUserInviteRequest request)
    {
        var result = await userService.FindUserInvite(request);

        return CreateObjectResult(result);
    }

    [HttpPost("invite/accept")]
    [AllowAnonymous]
    public async Task<ActionResult<AcceptUserInviteResponse>> AcceptInvite(AcceptUserInviteRequest request)
    {
        var result = await userService.AcceptUserInvite(request);

        return CreateObjectResult(result);
    }
}