
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
    protected readonly IConfiguration _config = config;

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