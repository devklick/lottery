using Microsoft.AspNetCore.Mvc;

namespace Lottery.Api.Models.Account.UpdateAccount;

public class UpdateAccountRequest
{
    [FromBody, BindProperty(Name = "")]
    public required UpdateAccountRequestBody Body { get; set; }
}