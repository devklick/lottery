using Lottery.DB.Context;
using Lottery.DB.Extensions;
using Lottery.Resulting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ResultingService = Lottery.Resulting.ResultService;

namespace Lottery.ResultService;

class Program
{
    async static Task Main(params string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddLogging();
        builder.Configuration.AddUserSecrets<Program>();
        builder.ConfigureEntityFramework<LotteryDBContext>("RESULTS_DB_USER", "RESULTS_DB_PASSWORD");
        builder.Services.AddScoped<ResultRepository>();
        builder.Services.AddScoped<ResultingService>();
        builder.Services.AddSingleton(TimeProvider.System);

        builder.Services.AddHostedService<ResultWorker>();

        var host = builder.Build();

        await host.RunAsync();
    }
}