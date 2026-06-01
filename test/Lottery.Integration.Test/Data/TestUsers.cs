using System.Data;

using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Identity;

namespace Lottery.Integration.Test.Data;

public static class TestUsers
{
    /// <summary>
    /// The app account that corresponds to the DB user which the tests will 
    /// connect to the DB with.
    /// </summary>
    public static readonly AppUser ServiceUser = new()
    {
        UserName = "postgres",
        NormalizedUserName = "postgres".ToUpperInvariant(),
        Id = Guid.Parse("8a869b1b-2d39-42e9-a3f9-c24ff101d768"),
        Email = "postgres@lottery.test",
        EmailConfirmed = true,
        LockoutEnabled = false,
        SecurityStamp = "f41a7b24363c4bbc96762bb5976d3eff"
    };

    public static readonly string AppUserName = "user";
    public static readonly string AppUserPassword = "user-password";
    /// <summary>
    /// The app account which tests will log into the app with.
    /// </summary>
    public static readonly AppUser AppUser = new()
    {
        UserName = AppUserName,
        NormalizedUserName = AppUserName.ToUpperInvariant(),
        Id = Guid.Parse("5df2b28c-0a0f-4075-a972-7912d5b113f7"),
        Email = "user@lottery.test",
        EmailConfirmed = true,
        SecurityStamp = "7a2de86b608d4ed29210006b8b80b210"
    };

    public static readonly string GameAdminUserName = "admin";
    public static readonly string GameAdminUserPassword = "admin-password";
    public static readonly AppUser GameAdminUser = new()
    {
        UserName = GameAdminUserName,
        NormalizedUserName = GameAdminUserName.ToUpperInvariant(),
        Id = Guid.Parse("1c41f977-1576-4821-91ad-5b56c80054db"),
        Email = "game.admin@lottery.test",
        EmailConfirmed = true,
        SecurityStamp = "373283d9b3cb427bbff338eaa3e48151"
    };

    public static readonly AppRole ServiceRole = new()
    {
        Id = Guid.Parse("66ab643e-0098-4c6a-a027-32e33d2b7a02"),
        Name = "Service",
        DisplayName = "Service",
        Description = "Role for service accounts to use",
    };
    public static readonly AppRole BasicRole = new()
    {
        Id = Guid.Parse("115b1a46-a50b-4e7a-906b-b7d7b25e7e45"),
        Name = "BasicUser",
        DisplayName = "Basic User",
        Description = "Basic user access",
    };

    public static readonly AppRole GameAdminRole = new()
    {
        Id = Guid.Parse("447104c9-021e-4414-a22a-126afc57ec73"),
        Name = "GameAdmin",
        DisplayName = "Game Admin",
        Description = "Game Admin",
    };

    static TestUsers()
    {
        var hasher = new PasswordHasher<AppUser>();
        AppUser.PasswordHash = hasher.HashPassword(AppUser, AppUserPassword);
        GameAdminUser.PasswordHash = hasher.HashPassword(GameAdminUser, GameAdminUserPassword);
    }
}