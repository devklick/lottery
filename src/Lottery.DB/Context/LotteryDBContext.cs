
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using Lottery.DB.Entities.Dbo;
using Lottery.DB.Entities.Ref;
using Lottery.Common.Extensions;
using Lottery.DB.Attributes;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using Lottery.DB.Entities.Idt;
using Lottery.DB.Entities.Base;

namespace Lottery.DB.Context;

public class LotteryDBContext(DbContextOptions options, IConfiguration config)
    : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>(options)
{
    private readonly IConfiguration _config = config;

    public DbSet<Game> Games { get; set; }
    public DbSet<GameSelection> GameSelections { get; set; }
    public DbSet<GameResult> GameResults { get; set; }
    public DbSet<GamePrize> GamePrizes { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public DbSet<EntrySelection> EntrySelections { get; set; }
    public DbSet<EntryPrize> EntryPrizes { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasPostgresEnum<ItemState>();

        foreach (var entity in builder.Model.GetEntityTypes())
        {
            RemoveInheritanceHierarchy(entity);

            ApplyNamingConvention(entity);

            AddCheckConstraint(entity);

            // foreach property/column
            foreach (var property in entity.GetProperties())
            {
                ApplyNamingConvention(property);

                AddDefaultConstraint(property);
            }

            // foreach foreign key
            foreach (var key in entity.GetForeignKeys())
            {
                // disable cascade delete
                key.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        SeedReferenceData(builder);

        SeedUsersAndRoles(builder);
    }

    private static void RemoveInheritanceHierarchy(IMutableEntityType entity)
    {
        entity.BaseType = null;
    }

    private static void ApplyNamingConvention(IMutableEntityType entity)
    {
        var tableAttribute = entity.ClrType.GetCustomAttribute<TableAttribute>();

        // apply snake casing on schema name
        var schemaName = (tableAttribute?.Schema ?? entity.GetSchema())?.ToSnakeCase();

        entity.SetSchema(schemaName);

        // apply snake casing on table name
        var tableName = (tableAttribute?.Name ?? entity.GetTableName())?.ToSnakeCase();

        entity.SetTableName(tableName);
    }

    private static void ApplyNamingConvention(IMutableProperty property)
    {
        // Replace column names with snake_case variant
        property.SetColumnName(property.Name.ToSnakeCase());
    }

    private static void AddDefaultConstraint(IMutableProperty property)
    {
        // apply default value
        var sqlDefault = property.PropertyInfo?.GetCustomAttribute<SqlColumnDefaultConstraintAttribute>();

        if (sqlDefault != null)
        {
            if (sqlDefault.IsSqlCommand)
            {
                property.SetDefaultValueSql(sqlDefault.DefaultValue.ToString());
            }
            else
            {
                property.SetDefaultValue(sqlDefault.DefaultValue);
            }
        }
    }

    private static void AddCheckConstraint(IMutableEntityType entity)
    {
        var checkConstraint = entity.ClrType.GetCustomAttribute<SqlTableCheckConstraintAttribute>();
        if (checkConstraint != null)
        {
            entity.AddCheckConstraint($"ch_{entity.GetSchema()}_{entity.GetTableName()}_{checkConstraint.ConstraintName}", checkConstraint.Sql);
        }
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

    public override int SaveChanges()
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is EntityObject entity)
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedOnUtc = now;
                    entity.UpdatedOnUtc = now;
                    entity.StateLastUpdatedUtc = now;
                }
                if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedOnUtc = now;

                    // TODO: Dunno about this, need to verify it works
                    if (entry.Collections.Any(c => c.Metadata.Name == nameof(EntityObject.State)))
                    {
                        entity.StateLastUpdatedUtc = now;
                    }
                }
            }
        }

        return base.SaveChanges();
    }
}