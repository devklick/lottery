using Lottery.DB.Entities.Idt;

using Lottery.Common.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Lottery.DB.Entities.Ref;

namespace Lottery.DB.Context;

/// <summary>
/// A context that should be used only for maintenance tasks.
/// 
/// This will take care of seeding data, allowing the standard DB context
/// to not require any knowledge of seed user passwords.
/// </summary>
internal class MaintenanceDBContext(DbContextOptions options, IConfiguration config) : LotteryDBContext(options, config)
{
    private readonly PasswordHasher<AppUser> _hasher = new();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        SeedReferenceData(builder);

        SeedUsersAndRoles(builder);
    }

    private static void SeedReferenceData(ModelBuilder builder)
    { }

    private void SeedUsersAndRoles(ModelBuilder builder)
    {
        var roleIds = new
        {
            systemAdmin = Guid.Parse("19b7d67e-1ad8-4407-b627-d5f56534952f"),
            gameAdmin = Guid.Parse("226919e5-1ad7-41d2-b04f-4aaa1a1bb2ea"),
            basicUser = Guid.Parse("5ca47808-83c0-4eab-a034-1a48cefa3c4a"),
            serviceAccount = Guid.Parse("db16d273-ae17-4822-bbf8-120cec7e3a58")
        };

        builder.Entity<AppRole>().HasData([
            new AppRole
            {
                Id = roleIds.systemAdmin,
                Name = "SystemAdministrator",
                NormalizedName = "SYSTEMADMINISTRATOR",
                DisplayName = "System Administrator",
                Description = "Elevated permissions across the entire system.",
                ConcurrencyStamp = Guid.NewGuid().ToString("N")
            },
            new AppRole
            {
                Id = roleIds.gameAdmin,
                Name = "GameAdmin",
                NormalizedName = "GAMEADMIN",
                DisplayName = "Game Admin",
                ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                Description = "Permission to create and edit any games",
            },
            new AppRole
            {
                Id = roleIds.basicUser,
                Name = "BasicUser",
                NormalizedName = "BASICUSER",
                DisplayName = "Basic User",
                ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                Description = "Permission to access the site and play games.",
            },
            new AppRole
            {
                Id = roleIds.serviceAccount,
                Name = "ServiceAccount",
                NormalizedName = "SERVICEACCOUNT",
                DisplayName = "Service Account",
                ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                Description = "Role to be assumed by user accounts used by backend services.",
            }
        ]);

        var userIds = new
        {
            systemAdmin = Guid.Parse("5621cc59-6211-42d2-a4e3-e9584c248adb"),
            gameAdmin = Guid.Parse("295c6034-e0ff-4c22-a94a-14fb4b6659a8"),
            resultService = Guid.Parse("aeb0bc13-14d4-4999-82c3-ec4b95a56818"),
            api = Guid.Parse("a3564302-1a9e-4917-8a48-1a70f211279e")
        };

        var userPasswords = new
        {
            systemAdmin = GetRequiredConfigValue("MaintenanceDBContext:SystemAdminPassword"),
            gameAdmin = GetRequiredConfigValue("MaintenanceDBContext:GameAdminPassword"),
            resultService = GetRequiredConfigValue("MaintenanceDBContext:ResultServicePassword"),
            api = GetRequiredConfigValue("MaintenanceDBContext:ApiPassword")
        };

        builder.Entity<AppUser>().HasData([
            // system admin
            CreateUser(new AppUser{
                Id = userIds.systemAdmin,
                Email = "SystemAdministrator@Lottery.Game",
                NormalizedEmail = "SYSTEMADMINISTRATOR@LOTTERY.GAME",
                EmailConfirmed = true,
                UserName = "SystemAdmin",
                NormalizedUserName = "SYSTEMADMIN",
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("N"),
                ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                AccountType = AccountType.User,
            }, userPasswords.systemAdmin),
            
            // game admin
            CreateUser(new AppUser{
                Id = userIds.gameAdmin,
                Email = "GameAdmin@Lottery.Game",
                NormalizedEmail = "GAMEADMIN@LOTTERY.GAME",
                EmailConfirmed = true,
                UserName = "GameAdmin",
                NormalizedUserName = "GAMEADMIN",
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("N"),
                ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                AccountType = AccountType.User,
            }, userPasswords.gameAdmin),
            
            // api
            CreateUser(new AppUser{
                Id = userIds.api,
                Email = "Lottery.Api@Lottery.Game",
                NormalizedEmail = "LOTTERY.API@LOTTERY.GAME",
                EmailConfirmed = true,
                UserName = "Lottery.Api",
                NormalizedUserName = "LOTTERY.API",
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("N"),
                ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                AccountType = AccountType.Service,
            }, userPasswords.api),

            // result service
            CreateUser(new AppUser{
                Id = userIds.resultService,
                Email = "Lottery.ResultService@Lottery.Game",
                NormalizedEmail = "LOTTERY.RESULTSERVICE@LOTTERY.GAME",
                EmailConfirmed = true,
                UserName = "Lottery.ResultService",
                NormalizedUserName = "LOTTERY.RESULTSERVICE",
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("N"),
                ConcurrencyStamp = Guid.NewGuid().ToString("N"),
                AccountType = AccountType.Service,
            }, userPasswords.resultService),
        ]);

        builder.Entity<AppUserRole>().HasData([
            new AppUserRole { RoleId = roleIds.systemAdmin, UserId = userIds.systemAdmin },
            new AppUserRole { RoleId = roleIds.gameAdmin, UserId = userIds.gameAdmin },
            new AppUserRole { RoleId = roleIds.serviceAccount, UserId = userIds.api },
            new AppUserRole { RoleId = roleIds.serviceAccount, UserId = userIds.resultService },
        ]);
    }

    private AppUser CreateUser(AppUser user, string password)
    {
        user.PasswordHash = _hasher.HashPassword(user, password);
        return user;
    }
}