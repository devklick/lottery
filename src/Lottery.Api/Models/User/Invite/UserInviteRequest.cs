using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.User.Invite;

public class UserInviteRequest
{
    [FromBody, BindProperty(Name = "")]
    public required UserInviteRequestBody Body { get; set; }
}