using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.UpdatePassword;

public class UpdatePasswordRequestBody
{
    [Required, FromBody]
    public required string CurrentPassword { get; set; }

    [Required, FromBody]
    public required string NewPassword { get; set; }
}