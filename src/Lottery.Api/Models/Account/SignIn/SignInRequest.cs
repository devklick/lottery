using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.SignIn;

public class SignInRequest
{
    [Required, FromBody, BindProperty(Name = "")]
    public required SignInRequestBody Body { get; set; }
}