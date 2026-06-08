using Lottery.Common.Helpers;
using Lottery.DB.Context;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Bootstrap.Seeding;

public static class DatabaseUsers
{
    public static async Task SeedAsync(LotteryDBContext context)
    {
        var db = Env.GetRequiredEnvVar("LOTTERY_POSTGRES_DB");
        // Create user & role for API
        var apiDBUser = Env.GetRequiredEnvVar("LOTTERY_API_DB_USER");
        var apiDBUserRole = $"{apiDBUser}_Role";
        await IdempotentCreateRole(context, db, apiDBUserRole, ["SELECT", "INSERT", "UPDATE", "DELETE"], ["dbo", "idt"]);
        await IdempotentCreateUser(context, apiDBUser, Env.GetRequiredEnvVar("LOTTERY_API_DB_PASSWORD"));
        await GrantRoleToUser(context, apiDBUserRole, apiDBUser);

        // Create user & role for result service
        var resultsDBUser = Env.GetRequiredEnvVar("LOTTERY_RESULTS_DB_USER");
        var resultsDBUserRole = $"{resultsDBUser}_Role";
        await IdempotentCreateRole(context, db, resultsDBUserRole, ["SELECT", "INSERT", "UPDATE", "DELETE"], ["dbo", "idt"]);
        await IdempotentCreateUser(context, resultsDBUser, Env.GetRequiredEnvVar("LOTTERY_RESULTS_DB_PASSWORD"));
        await GrantRoleToUser(context, resultsDBUserRole, resultsDBUser);
    }

    static async Task GrantRoleToUser(LotteryDBContext context, string role, string user)
    {
#pragma warning disable EF1002
        await context.Database.ExecuteSqlRawAsync(@$"
        DO
        $$
        BEGIN
            EXECUTE format('GRANT %I to %I', '{role}', '{user}');
        END
        $$
    ");
#pragma warning restore EF1002
    }

    static async Task IdempotentCreateRole(LotteryDBContext context, string database, string roleName, string[] privileges, string[] schemas)
    {
        // TODO: This fails to find a role that already exists
        // possibly something to do with t
        var exists = context.Database.SqlQuery<bool>($"SELECT true FROM pg_catalog.pg_roles WHERE rolname = {roleName}");

        // We have to use raw SQL here since the values are parameterized but are identifiers, not SQL values.
        // The privileges and schemas can be trusted, but everything else is provided externally.
#pragma warning disable EF1002
        if (!exists.Any())
        {
            await context.Database.ExecuteSqlRawAsync($@"
            DO
            $$
            BEGIN
                EXECUTE format('CREATE ROLE %I', '{roleName}');
                EXECUTE format('GRANT CONNECT ON DATABASE %I TO %I', '{database}', '{roleName}');
                EXECUTE format('GRANT USAGE ON SCHEMA {string.Join(',', schemas)} TO  %I', '{roleName}');
                EXECUTE format('GRANT {string.Join(',', privileges)} ON ALL TABLES IN SCHEMA {string.Join(',', schemas)} TO %I', '{roleName}');
            END
            $$
        ");
        }
#pragma warning restore EF1002
    }
    static async Task IdempotentCreateUser(LotteryDBContext context, string username, string password)
    {
        var exists = context.Database.SqlQuery<bool>($"SELECT true FROM pg_catalog.pg_user WHERE usename = {username}");
        // We have to use raw SQL here since the values are parameterized but are identifiers, not SQL values
#pragma warning disable EF1002
        if (!exists.Any())
        {
            await context.Database.ExecuteSqlRawAsync(@$"
            DO
            $$
            BEGIN
                EXECUTE format('CREATE USER %I WITH PASSWORD %L', '{username}', '{password}');
            END
            $$
        ");
        }
#pragma warning restore EF1002
    }

}