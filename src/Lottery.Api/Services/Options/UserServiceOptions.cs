namespace Lottery.Api.Services.Options;

public class UserServiceOptions
{
    public static readonly string Name = nameof(UserServiceOptions);
    public bool AutoConfirmNewAccounts { get; set; }
}