using Lottery.Common.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lottery.ResultService;

using ResultingService = Resulting.ResultService;

public class ResultWorker(IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            var service = scope.ServiceProvider.GetRequiredService<ResultingService>();


            var games = await service.GetGamesToResult();

            foreach (var game in games)
            {
                var result = await service.ResultGame(game, []);

                if (result.Status != ResultStatus.Ok || result.Value is null)
                {
                    var errors = result.Errors ?? [new() { Message = "Unknown error" }];
                    throw new Exception($"Resulting failed for game {game.Id}. Errors: {string.Join(". ", errors)}");
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}