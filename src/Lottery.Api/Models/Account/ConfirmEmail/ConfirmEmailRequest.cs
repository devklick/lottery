using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.ConfirmEmail;

public class ConfirmEmailRequest
{
    [Required, FromQuery, BindProperty(Name = "")]
    public required ConfirmEmailRequestQuery Query { get; set; }
}