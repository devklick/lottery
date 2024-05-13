using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.User.Invite.Accept;

public class AcceptUserInviteRequestBody
{
    [Required, DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }

    [Required, MinLength(128), MaxLength(128)]
    public required string Token { get; set; }

}