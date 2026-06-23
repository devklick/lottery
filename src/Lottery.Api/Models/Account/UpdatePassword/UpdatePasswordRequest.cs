using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.UpdatePassword;

public class UpdatePasswordRequest
{
    [Required, FromBody, BindProperty(Name = "")]
    public required UpdatePasswordRequestBody Body { get; set; }
}