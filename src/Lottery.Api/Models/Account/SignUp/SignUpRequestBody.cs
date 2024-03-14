using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.Account.SignUp;

public class SignUpRequestBody
{
    [Required, DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}