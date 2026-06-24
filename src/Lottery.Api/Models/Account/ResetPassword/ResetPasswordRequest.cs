using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.ResetPassword;

public class ResetPasswordRequest
{
    [Required, FromBody, BindProperty(Name = "")]
    public required ResetPasswordRequestBody Body { get; set; }
}