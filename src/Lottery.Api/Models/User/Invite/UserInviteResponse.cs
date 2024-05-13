namespace Lottery.Api.Models.User.Invite;

public class UserInviteResponse
{
    // Ideally we wouldnt return this information to the user who 
    // is inviting another user, we'd just email it to the other user. 
    // However I have no plan to implement the email, so this will help 
    // with testing
    public required string Email { get; set; }
    public required string Token { get; set; }
}