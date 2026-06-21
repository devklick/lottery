namespace Lottery.Api.Services.Options;

public class UserServiceOptions
{
    public static readonly string Name = nameof(UserServiceOptions);
    public bool AutoConfirmNewAccounts { get; set; }
    public TimeSpan UserInviteValidFor { get; set; } = TimeSpan.FromHours(24);
    public required string EmailConfirmationDomain { get; set; }
}