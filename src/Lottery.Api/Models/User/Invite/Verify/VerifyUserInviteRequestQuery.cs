using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.User.Invite.Verify;

public class VerifyUserInviteRequestQuery
{
    [Required, DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [Required, MinLength(128), MaxLength(128)]
    public required string Token { get; set; }
}