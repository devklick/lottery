using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.User.Invite.Verify;

public class VerifyUserInviteRequest
{
    [Required, FromQuery, BindProperty(Name = "")]
    public required VerifyUserInviteRequestQuery Query { get; set; }
}