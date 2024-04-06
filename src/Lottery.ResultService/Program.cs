using Lottery.DB.Context;
using Lottery.DB.Extensions;
using Lottery.Resulting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = SetupHost();

using var serviceScope = host.Services.CreateScope();
var service = serviceScope.ServiceProvider.GetRequiredService<ResultService>();

var games = await service.GetGamesToResult();

foreach (var game in games)
{
    await service.ResultGame(game, []);
}

IHost SetupHost()
{
    var builder = Host.CreateApplicationBuilder(args);

    builder.Configuration.AddUserSecrets<Program>();
    builder.ConfigureEntityFramework<LotteryDBContext>();
    builder.Services.AddSingleton<ResultRepository>();
    builder.Services.AddSingleton<ResultService>();

    return builder.Build();
}