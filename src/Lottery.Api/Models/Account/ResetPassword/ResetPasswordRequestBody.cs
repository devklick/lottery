using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.ResetPassword;

public class ResetPasswordRequestBody
{
    [Required, FromBody]
    public required string Email { get; set; }

    [Required, FromBody]
    public required string Password { get; set; }

    [Required, FromBody]
    public required string Token { get; set; }
}