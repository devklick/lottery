using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.Account.ConfirmPassword;

public class ConfirmPasswordRequestBody
{
    [Required]
    public required string Password { get; set; }
}