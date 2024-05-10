using System.ComponentModel.DataAnnotations;

using Lottery.Api.Models.Common;

namespace Lottery.Api.Models.User.Invite;

public class UserInviteRequestBody
{
    [Required, StringLength(360)]
    public required string Email { get; set; }

    [Required, AllowedValues(UserType.Admin, UserType.SystemAdmin, UserType.Basic)]
    public required UserType UserType { get; set; }
}