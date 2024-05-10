using Lottery.Api.Models.Common;
using Lottery.DB.Context;
using Lottery.DB.Entities.Idt;
using Lottery.DB.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Lottery.Api.Repositories.User;

public class UserRepository(LotteryDBContext db) : RepositoryBase<LotteryDBContext>(db)
{
    public async Task<AppUserInvite> CreateUserInvite(AppUserInvite invite)
    {
        await _db.UserInvites.AddAsync(invite);

        return invite;
    }

    public async Task<AppUserInviteRole> CreateUserInviteRoles(AppUserInviteRole inviteRole)
    {
        await _db.AddAsync(inviteRole);
        return inviteRole;
    }

    public async Task<AppRole?> GetRole(UserType userType)
        => await _db.Roles.FirstOrDefaultAsync(x => x.Name == GetRoleName(userType));

    private static string GetRoleName(UserType userType) => userType switch
    {
        UserType.Admin => "GameAdmin",
        UserType.Basic => "BasicUser",
        UserType.SystemAdmin => "SystemAdministrator",
        UserType.Guest => throw new NotImplementedException($"User type {userType} has no known roles"),
        _ => throw new NotImplementedException($"User type {userType} not known"),
    };
}