using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Entities.Idt;

[Table(nameof(AppUserInviteRole), Schema = nameof(Idt))]
[PrimaryKey(nameof(AppUserInviteId), nameof(AppRoleId))]
public class AppUserInviteRole
{
    public Guid AppUserInviteId { get; set; }
    public Guid AppRoleId { get; set; }

    public AppUserInvite AppUserInvite { get; set; } = default!;
    public AppRole AppRole { get; set; } = default!;
}