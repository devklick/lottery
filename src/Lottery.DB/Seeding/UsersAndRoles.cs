using Lottery.Common.Helpers;
using Lottery.DB.Entities.Idt;
using Lottery.DB.Entities.Ref;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Seeding;

internal static class UsersAndRoles
{
    public static void Seed(DbContext context, Microsoft.AspNetCore.Identity.IPasswordHasher<AppUser> hasher)
    {

        var systemAdminRole = new AppRole
        {
            Id = Guid.Parse("19b7d67e-1ad8-4407-b627-d5f56534952f"),
            Name = "SystemAdministrator",
            NormalizedName = "SYSTEMADMINISTRATOR",
            DisplayName = "System Administrator",
            Description = "Elevated permissions across the entire system.",
            ConcurrencyStamp = "b10a7e5e8875420a8d90a55e38b11fb0"
        };
        var gameAdminRole = new AppRole
        {
            Id = Guid.Parse("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"),
            Name = "GameAdmin",
            NormalizedName = "GAMEADMIN",
            DisplayName = "Game Admin",
            ConcurrencyStamp = "76c43f385c2b405fba3d946f2bcea6b1",
            Description = "Permission to create and edit any games",
        };
        var basicUserRole = new AppRole
        {
            Id = Guid.Parse("5ca47808-83c0-4eab-a034-1a48cefa3c4a"),
            Name = "BasicUser",
            NormalizedName = "BASICUSER",
            DisplayName = "Basic User",
            ConcurrencyStamp = "4cdc4513a4804f46aa2ef1538249c2d1",
            Description = "Permission to access the site and play games.",
        };
        var serviceAccountRole = new AppRole
        {
            Id = Guid.Parse("db16d273-ae17-4822-bbf8-120cec7e3a58"),
            Name = "ServiceAccount",
            NormalizedName = "SERVICEACCOUNT",
            DisplayName = "Service Account",
            ConcurrencyStamp = "0b1e458292f14381be395229b68a6e3c",
            Description = "Role to be assumed by user accounts used by backend services.",
        };


        // TODO: Hardcoded password hashes here are no good. 
        // The password is specified via env var, so these hashes need to be dynamic. 
        // However, dynamic seed data may cause problems 
        // (generating a password hash will change every time the seeding runs)
        var systemAdminUser = new AppUser
        {
            Id = Guid.Parse("5621cc59-6211-42d2-a4e3-e9584c248adb"),
            Email = "SystemAdministrator@Lottery.Game",
            NormalizedEmail = "SYSTEMADMINISTRATOR@LOTTERY.GAME",
            EmailConfirmed = true,
            UserName = "SystemAdmin",
            NormalizedUserName = "SYSTEMADMIN",
            LockoutEnabled = false,
            SecurityStamp = "6cd1f1703ac548e2a9295d2568fdc229",
            ConcurrencyStamp = "421123fe685b4be1a9b63c6d809583e5",
            AccountType = AccountType.User,
        };
        systemAdminUser.PasswordHash = hasher.HashPassword(systemAdminUser, Env.GetRequiredEnvVar("SYSTEM_ADMIN_PASSWORD")); //"AQAAAAEAACcQAAAAEMBfcZEx4+P6j8kjngjP548MXLwVIEC4bSHvrvHZ7CtTNNaHwcofmAy8kHcQ8eT64w=="

        var gameAdminUser = new AppUser
        {
            Id = Guid.Parse("295c6034-e0ff-4c22-a94a-14fb4b6659a8"),
            Email = "GameAdmin@Lottery.Game",
            NormalizedEmail = "GAMEADMIN@LOTTERY.GAME",
            EmailConfirmed = true,
            UserName = "GameAdmin",
            NormalizedUserName = "GAMEADMIN",
            LockoutEnabled = false,
            SecurityStamp = "0c057e3ddbb746aa9fd1ad3f9b98488d",
            ConcurrencyStamp = "d2761ece200c4ead95f643a5227218a5",
            AccountType = AccountType.User,
        };
        gameAdminUser.PasswordHash = hasher.HashPassword(gameAdminUser, Env.GetRequiredEnvVar("GAME_ADMIN_PASSWORD")); //"AQAAAAEAACcQAAAAELjUDpUY+Ew4tf3+b2aD4PB5dHyOllNrAhl10GpgXC49Qo4Rl1bthnXm/wD1Dry7Qw=="

        var apiUser = new AppUser
        {
            Id = Guid.Parse("a3564302-1a9e-4917-8a48-1a70f211279e"),
            Email = "Lottery.Api.User@Lottery.Game",
            NormalizedEmail = "LOTTERY.API.USER@LOTTERY.GAME",
            EmailConfirmed = true,
            UserName = Env.GetRequiredEnvVar("API_DB_USER"),
            NormalizedUserName = Env.GetRequiredEnvVar("API_DB_USER").ToUpperInvariant(),
            LockoutEnabled = false,
            SecurityStamp = "801f9eb0a8cf40fc8a8c621d70ffe214",
            ConcurrencyStamp = "ConcurrencyStamp",
            AccountType = AccountType.Service,
            // no app account passwords for service accounts, 
            // as we dont want to be able to log in as this account in the app
            PasswordHash = null
        };

        var resultsUser = new AppUser
        {
            Id = Guid.Parse("aeb0bc13-14d4-4999-82c3-ec4b95a56818"),
            Email = "Lottery.ResultService.User@Lottery.Game",
            NormalizedEmail = "LOTTERY.RESULTSERVICE.USER@LOTTERY.GAME",
            EmailConfirmed = true,
            UserName = Env.GetRequiredEnvVar("RESULTS_DB_USER"),
            NormalizedUserName = Env.GetRequiredEnvVar("RESULTS_DB_USER").ToUpperInvariant(),
            LockoutEnabled = false,
            SecurityStamp = "bdd00b91be02492cb91154dabb5ce5a2",
            ConcurrencyStamp = "87d73fa701bf494b9ef9c5f193278a3f",
            AccountType = AccountType.Service,
            // no app account passwords for service accounts, 
            // as we dont want to be able to log in as this account in the app
            PasswordHash = null
        };

        IEnumerable<AppRole> roles = [systemAdminRole, gameAdminRole, basicUserRole, serviceAccountRole];
        IEnumerable<AppUser> users = [systemAdminUser, gameAdminUser, apiUser, resultsUser];
        IEnumerable<AppUserRole> appUserRoles = [
            new AppUserRole { RoleId = systemAdminRole.Id, UserId = systemAdminUser.Id },
            new AppUserRole { RoleId = gameAdminRole.Id, UserId = gameAdminUser.Id },
            new AppUserRole { RoleId = serviceAccountRole.Id, UserId = apiUser.Id },
            new AppUserRole { RoleId = serviceAccountRole.Id, UserId = resultsUser.Id },
        ];
        foreach (var role in roles)
        {
            var set = context.Set<AppRole>();
            var existing = set.FirstOrDefault(r => r.Id == role.Id);
            if (existing is null) context.Set<AppRole>().Add(role);
        }
        foreach (var user in users)
        {
            var set = context.Set<AppUser>();
            var existing = set.FirstOrDefault(r => r.Id == user.Id);
            if (existing is null) context.Set<AppUser>().Add(user);
        }
        foreach (var userRole in appUserRoles)
        {
            var set = context.Set<AppUserRole>();
            var existing = set.FirstOrDefault(r => r.UserId == userRole.UserId && r.RoleId == userRole.RoleId);
            if (existing is null) context.Set<AppUserRole>().Add(userRole);
        }

        context.SaveChanges();
    }
}
