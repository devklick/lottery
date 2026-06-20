namespace Lottery.Api.Models.Account.GetAccount;

public class GetAccountResponse
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string? PhoneNumber { get; set; }
    public required bool EmailConfirmed { get; set; }
    public required bool PhoneNumberConfirmed { get; set; }
}