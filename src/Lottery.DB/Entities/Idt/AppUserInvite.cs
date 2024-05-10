using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Entities.Base;
using Lottery.DB.Entities.Ref;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Entities.Idt;

/// <summary>
/// When one user invites another, an AppUserInvite is created
/// </summary>
[Table(nameof(AppUserInvite), Schema = nameof(Idt))]
// We'll allow a single email to be invited only once. 
// If we need to re-send the invite, we'll generate a new token for the existing invite
[Index(nameof(Email), IsUnique = true)]
public class AppUserInvite : EntityObject
{
    /// <summary>
    /// The email address which has been invited
    /// </summary>
    [StringLength(320)]
    public required string Email { get; set; }

    /// <summary>
    /// The token that is used to verify the invite
    /// </summary>
    [StringLength(128)]
    public required string Token { get; set; }

    /// <summary>
    /// The time at which the invite expires
    /// </summary>
    public DateTime Expiry { get; set; }

    public AccountType AccountType { get; set; } = AccountType.User;

    /// <summary>
    /// The ID of the user created on the back of accepting the invite, 
    /// if it was accepted. If it was not accepted before the Expiry, 
    /// the AppUserId will be null.
    /// </summary>
    public Guid? AppUserId { get; set; }

    public AppUser? AppUser { get; set; }
    public List<AppUserInviteRole> AppUserInviteRoles { get; set; } = [];
}