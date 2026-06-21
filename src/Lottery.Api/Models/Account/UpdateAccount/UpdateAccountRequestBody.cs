using Lottery.Api.Models.Validation;

namespace Lottery.Api.Models.Account.UpdateAccount;

[RequireOneOrMore(nameof(Username), nameof(Email), nameof(PhoneNumber))]
public class UpdateAccountRequestBody
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}