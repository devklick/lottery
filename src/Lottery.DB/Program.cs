using Lottery.DB.Context;
using Lottery.DB.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

/*
    Simple program to allow the Lottery.DB project to act as a 
    stand-alone place to manage the DB and migrations.

    Example usage:
        dotnet ef migrations add InitialMigration --project ./src/Lottery.DB
*/

var builder = Host.CreateApplicationBuilder(args);
builder.ConfigureEntityFramework<LotteryDBContext>();
builder.Configuration.AddUserSecrets<Program>();
builder.Build();