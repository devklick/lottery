using Lottery.Api.Models.Common;

namespace Lottery.Api.Models.Account.SignIn;

public class SignInResponse
{
    public required UserType UserType { get; set; }
    public required DateTime SessionExpiry { get; set; }
}