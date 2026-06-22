using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.ConfirmEmail;

public class ConfirmEmailRequestQuery
{

    [Required, FromQuery]
    public required string Token { get; set; }

    [Required, FromQuery]
    public required Guid UserId { get; set; }
}