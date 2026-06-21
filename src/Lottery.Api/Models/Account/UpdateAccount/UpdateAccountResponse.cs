using Lottery.Common.Models;

namespace Lottery.Api.Models.Account.UpdateAccount;

public class UpdateAccountResponse
{
    public required Result<string> Username { get; set; }
    public required Result<string> Email { get; set; }
    public required Result<string?> PhoneNumber { get; set; }
}