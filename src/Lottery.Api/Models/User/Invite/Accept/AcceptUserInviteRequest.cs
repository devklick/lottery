using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.User.Invite.Accept;


public class AcceptUserInviteRequest
{
    [Required, FromBody, BindProperty(Name = "")]
    public required AcceptUserInviteRequestBody Body { get; set; }
}