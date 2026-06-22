using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.ConfirmPassword;

public class ConfirmPasswordRequest
{
    [Required, FromBody, BindProperty(Name = "")]
    public required ConfirmPasswordRequestBody Body { get; set; }
}