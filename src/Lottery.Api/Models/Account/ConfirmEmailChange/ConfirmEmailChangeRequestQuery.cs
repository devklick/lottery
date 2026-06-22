using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.ConfirmEmailChange;

public class ConfirmEmailChangeRequestQuery
{
    [Required, FromQuery]
    public required string Email { get; set; }

    [Required, FromQuery]
    public required string Token { get; set; }

    [Required, FromQuery]
    public required Guid UserId { get; set; }
}