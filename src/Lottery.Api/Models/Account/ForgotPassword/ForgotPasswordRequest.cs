using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.ForgotPassword;

public class ForgotPasswordRequest
{
    [Required, FromBody, BindProperty(Name = "")]
    public required ForgotPasswordRequestBody Body { get; set; }
}