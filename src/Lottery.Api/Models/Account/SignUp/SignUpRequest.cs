using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.SignUp;

public class SignUpRequest
{
    [Required, FromBody, BindProperty(Name = "")]
    public required SignUpRequestBody Body { get; set; }
}