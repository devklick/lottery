using Lottery.DB.Entities.Idt;

using Lottery.Common.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace Lottery.DB.Context;

/// <summary>
/// A context that should be used only for maintenance tasks.
/// 
/// This will take care of seeding data, allowing the standard DB context
/// to not require any knowledge of seed user passwords.
/// </summary>
internal class MaintenanceDBContext(DbContextOptions options, IConfiguration config) : LotteryDBContext(options, config)
{
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
        var hasher = new PasswordHasher<AppUser>();

        // grab SA password - this should be in secrets file and added to config
        var saPass = _config["LotteryDBContext:SystemAdminPassword"];
        if (saPass.IsNullOrEmpty())
        {
            throw new InvalidOperationException(
                "Unable to find the password for the SystemAdmin account. Make sure this is present in user secrets.");
        }

        // grab SU password - this should be in secrets file and added to config
        var gaPass = _config["LotteryDBContext:GameAdminPassword"];
        if (gaPass.IsNullOrEmpty())
        {
            throw new InvalidOperationException(
                "Unable to find the password for the SuperUser account. Make sure this is present in user secrets.");
        }

        // Create the System Administrator role
        var saRoleId = Guid.NewGuid();
        builder.Entity<AppRole>().HasData(new AppRole
        {
            Id = saRoleId,
            Name = "SystemAdministrator",
            NormalizedName = "SYSTEMADMINISTRATOR",
            DisplayName = "System Administrator",
            Description = "Elevated permissions across the entire system.",
            ConcurrencyStamp = Guid.NewGuid().ToString("N")
        });

        // Create a single System Administrator user account
        var saUserId = Guid.NewGuid();
        var saUser = new AppUser
        {
            Id = saUserId,
            Email = "SystemAdministrator@Lottery.Game",
            NormalizedEmail = "SYSTEMADMINISTRATOR@LOTTERY.GAME",
            EmailConfirmed = true,
            UserName = "SystemAdmin",
            NormalizedUserName = "SYSTEMADMIN",
            LockoutEnabled = false,
            SecurityStamp = Guid.NewGuid().ToString("N"),
            ConcurrencyStamp = Guid.NewGuid().ToString("N")
        };
        saUser.PasswordHash = hasher.HashPassword(saUser, saPass);
        builder.Entity<AppUser>().HasData(saUser);

        // Give the System Administrator user the System Administrator role
        builder.Entity<AppUserRole>().HasData(new AppUserRole
        {
            RoleId = saRoleId,
            UserId = saUserId
        });

        // Create the Super User role
        var suRoleId = Guid.NewGuid();
        builder.Entity<AppRole>().HasData(new AppRole
        {
            Id = suRoleId,
            Name = "GameAdmin",
            NormalizedName = "GAMEADMIN",
            DisplayName = "Game Admin",
            ConcurrencyStamp = Guid.NewGuid().ToString("N"),
            Description = "Permission to create and edit any games",
        });

        // Create a single Super User account
        var gaUserId = Guid.NewGuid();
        var gaUser = new AppUser
        {
            Id = gaUserId,
            Email = "GameAdmin@Lottery.Game",
            NormalizedEmail = "GAMEADMIN@LOTTERY.GAME",
            EmailConfirmed = true,
            UserName = "GameAdmin",
            NormalizedUserName = "GAMEADMIN",
            LockoutEnabled = false,
            SecurityStamp = Guid.NewGuid().ToString("N"),
            ConcurrencyStamp = Guid.NewGuid().ToString("N")
        };
        gaUser.PasswordHash = hasher.HashPassword(gaUser, gaPass);
        builder.Entity<AppUser>().HasData(gaUser);

        // Give the Super User account the Super User role
        builder.Entity<AppUserRole>().HasData(new AppUserRole
        {
            RoleId = suRoleId,
            UserId = gaUserId
        });

        // Create the Basic User role
        var buRoleId = Guid.NewGuid();
        builder.Entity<AppRole>().HasData(new AppRole
        {
            Id = buRoleId,
            Name = "BasicUser",
            NormalizedName = "BASICUSER",
            DisplayName = "Basic User",
            ConcurrencyStamp = Guid.NewGuid().ToString("N"),
            Description = "Permission to access the site and play games.",
        });
    }

}