using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.ConfirmEmailChange;

public class ConfirmEmailChangeRequest
{
    [Required, BindProperty(Name = "")]
    public required ConfirmEmailChangeRequestQuery Query { get; set; }
}