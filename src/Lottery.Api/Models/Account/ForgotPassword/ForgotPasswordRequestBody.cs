using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.ForgotPassword;

public class ForgotPasswordRequestBody
{
    [Required, FromBody]
    public required string Email { get; set; }
}