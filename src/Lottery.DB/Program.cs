using Lottery.DB.Context;
using Lottery.DB.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

/*
    Simple program to allow the Lottery.DB project to act as a 
    stand-alone place to manage the DB and migrations.

    Example usage:
        dotnet ef migrations add InitialMigration --project ./src/Lottery.DB --context MaintenanceDBContext

    Note that since multiple DBContexts exist in this repo, we have to specify which one
    should be used when invoking the dotnet command, using the --context argument.
*/

var builder = Host.CreateApplicationBuilder(args);
builder.ConfigureEntityFramework<MaintenanceDBContext>();
builder.Configuration.AddUserSecrets<Program>();
builder.Build();