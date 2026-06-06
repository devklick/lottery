using DotNetEnv.Configuration;

using Lottery.DB.Bootstrap.Seeding;
using Lottery.DB.Context;
using Lottery.DB.Entities.Idt;
using Lottery.DB.Extensions;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var hasher = new PasswordHasher<AppUser>();

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddDotNetEnv("../../.env");
builder.ConfigureEntityFramework<LotteryDBContext>("LOTTERY_MIGRATOR_DB_USER", "LOTTERY_MIGRATOR_DB_PASSWORD");
var host = builder.Build();

var context = host.Services.GetRequiredService<LotteryDBContext>();

await context.Database.MigrateAsync();

await DatabaseUsers.SeedAsync(context);
await ReferenceData.SeedAsync(context);
await AppUsers.SeedAsync(context, hasher);