using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.Account.SignIn;

public class SignInRequestBody
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }

    [DefaultValue(false)]
    public bool StaySignedIn { get; set; }
}