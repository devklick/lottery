
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using Lottery.Repository.Entities.Dbo;
using Lottery.Repository.Entities.Ref;
using Lottery.Common.Extensions;
using Lottery.Repository.Attributes;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using Lottery.Repository.Entities.Idt;
using Lottery.Repository.Entities.Base;

namespace Lottery.Repository.Context;

public class LotteryDBContext(DbContextOptions options, IConfiguration config)
    : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>(options)
{
    private readonly IConfiguration _config = config;

    public DbSet<Game> Games { get; set; }
    public DbSet<Entry> Entries { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        Console.WriteLine("Base model creation");
        base.OnModelCreating(builder);

        Console.WriteLine("Adding postgres enums");
        builder.HasPostgresEnum<ItemState>();

        Console.WriteLine("Iterating entities");
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            Console.WriteLine("Removing inheritance");
            RemoveInheritanceHierarchy(entity);

            Console.WriteLine("Applying entity name");
            ApplyNamingConvention(entity);

            Console.WriteLine("Adding check constraints");
            AddCheckConstraint(entity);

            Console.WriteLine("Adding unique constraints");
            AddUniqueConstraint(entity);

            Console.WriteLine("Adding composite keys");
            AddCompositeKey(entity);


            Console.WriteLine("Iterating properties");
            // foreach property/column
            foreach (var property in entity.GetProperties())
            {
                Console.WriteLine("Applying property name");
                ApplyNamingConvention(property);

                Console.WriteLine("Applying default constraint");
                AddDefaultConstraint(property);

                Console.WriteLine("Applying unique constraint");
                AddUniqueConstraint(property, entity);
            }

            // foreach foreign key
            foreach (var key in entity.GetForeignKeys())
            {
                // disable cascade delete
                key.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        // builder.Entity<AppUserRole>().HasKey(e => new { e.UserId, e.RoleId });

        SeedReferenceData(builder);

        SeedUsersAndRoles(builder);
    }

    private static void AddCompositeKey(IMutableEntityType entity)
    {
        var attr = entity.ClrType.GetCustomAttribute<CompositeKeyAttribute>();
        if (attr == null) return;

        var props = entity.GetProperties();

        entity.SetPrimaryKey(new List<IMutableProperty>(attr.ColumnNames.Select(cn => props.First(p => p.Name == cn))));


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

    private static void AddUniqueConstraint(IMutableEntityType entity)
    {
        var tableLevelUniques = entity.ClrType.GetCustomAttributes<SqlTableUniqueIndexAttribute>();

        if (tableLevelUniques != null)
        {
            foreach (var tableLevelUnique in tableLevelUniques)
            {
                var props = new List<IMutableProperty>();
                foreach (string propName in tableLevelUnique.PropertyNames)
                {
                    var prop = entity.GetProperties().First(p => p.Name == propName);
                    props.Add(prop);
                }
                entity.AddIndex(props).IsUnique = true;

                if (!string.IsNullOrEmpty(tableLevelUnique.ConstraintName))
                {
                    entity.FindIndex(props)?.SetDatabaseName(tableLevelUnique.ConstraintName);
                }
            }
        }
    }

    private void AddUniqueConstraint(IMutableProperty property, IMutableEntityType entity)
    {
        // apply column level unique unique constraints
        var unique = property.PropertyInfo?.GetCustomAttribute<SqlColumnUniqueConstraintAttribute>();
        if (unique != null)
        {
            entity.AddIndex(property).IsUnique = true;
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