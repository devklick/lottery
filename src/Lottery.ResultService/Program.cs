using System.Security.Cryptography;

using Lottery.DB.Configuration;
using Lottery.DB.Context;
using Lottery.DB.Entities.Dbo;
using Lottery.ResultService.Repositories;
using Lottery.ResultService.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

ConfigureEntityFramework(builder);

builder.Services.AddSingleton<ResultRepository>();

var host = builder.Build();
var serviceScope = host.Services.CreateScope();
var repo = serviceScope.ServiceProvider.GetRequiredService<ResultRepository>();

var games = await repo.GetGamesToResult();
var adminUserId = await repo.GetSystemAdminUserId();

foreach (var game in games)
{
    // Determine how many numbers need to be picked
    var numbersToDraw = game.Prizes.Find(p => p.Position == 1)?.NumberMatchCount
        ?? throw new Exception($"Game {game.Id} does not have a prize for position 1");

    // Pick the winning numbers (numbers are stored as GameSelections)
    var winningSelections = Rng.TakeRandom(game.Selections, numbersToDraw);

    // Store the winning numbers
    var gameResults = winningSelections.Select(s => new GameResult
    {
        GameId = game.Id,
        SelectionId = s.Id,
        CreatedById = adminUserId
    });

    await repo.AddGameResults(gameResults);

    // Save the results at this point
    await repo.SaveChangesAsync();

    // Determine the entries who have won prizes.
    // We'll get a list of back with an item for each prize, along with the 
    // entries who have won that prize (if any)
    var prizeWinners = await repo.GetPrizeWinners(game.Id, winningSelections.Select(s => s.Id));

    var entryPrizes = new List<EntryPrize>();
    // Cycle through the prizes and process the winners
    foreach (var (gamePrize, winningEntries) in prizeWinners)
    {
        // If there's no winners for this prize, move on
        if (!winningEntries.Any()) continue;

        // Construct the entry prizes that link a game prize to a player
        entryPrizes.AddRange(winningEntries.Select(we => new EntryPrize
        {
            GameId = game.Id,
            GamePrizeId = gamePrize.Id,
            EntryId = we.Id,
            CreatedById = adminUserId
        }));
    }

    // Store the entry prizes
    await repo.AddEntryPrizes(entryPrizes);

    await repo.SaveChangesAsync();
}

static void ConfigureEntityFramework(HostApplicationBuilder builder)
{
    builder.Services.Configure<EFMigrationSettings>(
        builder.Configuration.GetSection(nameof(EFMigrationSettings)));

    builder.Services.AddDbContext<LotteryDBContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("Default")
            ?? throw new Exception("No default connection string found");

        var dbPassword = builder.Configuration["ConnectionStrings:Default:Password"];

        if (!string.IsNullOrWhiteSpace(dbPassword))
        {
            connectionString = connectionString.TrimEnd();
            connectionString += $";Password={dbPassword};";
        }
        options.UseNpgsql(connectionString, sqlOps =>
        {
            var settings = builder.Configuration.GetSection(nameof(EFMigrationSettings)).Get<EFMigrationSettings>()
                ?? throw new Exception("No EFMigrationSettings found");

            sqlOps.MigrationsHistoryTable(tableName: settings.TableName, schema: settings.SchemaName);
        });
    });
}