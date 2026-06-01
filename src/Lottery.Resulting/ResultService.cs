using Lottery.Common.Models;
using Lottery.DB.Entities.Dbo;

using Microsoft.Extensions.Logging;

namespace Lottery.Resulting;

public class ResultService(ResultRepository repository, ILogger<ResultService> logger, TimeProvider timeProvider)
{
    private readonly ResultRepository _repository = repository;
    private readonly ILogger<ResultService> _logger = logger;


    public async Task<IEnumerable<Game>> GetGamesToResult()
    {
        _logger.LogTrace(nameof(GetGamesToResult));
        var games = await _repository.GetGamesToResult();
        _logger.LogInformation("Found {count} games to result", games.Count);
        return games;
    }

    public async Task<Result<Game>> ResultGame(Game game, IEnumerable<int> winningNumbers)
    {
        _logger.LogTrace("{method} {gameId}, [{winningNumbers}]", nameof(ResultGame), game.Id, string.Join(',', winningNumbers));

        if (game.GameStatus != GameStatus.Closed)
        {
            return new Result<Game>
            {
                Status = ResultStatus.BadRequest,
                Errors = [new() { Message = $"Unable to result game {game.Id} while its in a {game.GameStatus} state" }]
            };
        }

        // Determine how many numbers need to be picked
        var numbersToDraw = game.Prizes.Find(p => p.Position == 1)?.NumberMatchCount
            ?? throw new Exception($"Game {game.Id} does not have a prize for position 1");

        // If winning numbers have been specified but are invalid, return error
        if (winningNumbers.Any() && winningNumbers.Count() != numbersToDraw)
        {
            return new Result<Game>
            {
                Status = ResultStatus.BadRequest,
                Errors = [new() { Message = $"Expected 0 or {numbersToDraw} winning numbers, found {winningNumbers.Count()}" }]
            };
        }
        else if (!winningNumbers.Any() && game.Selections.Count < numbersToDraw)
        {
            return new Result<Game>
            {
                Status = ResultStatus.BadRequest,
                Errors = [new() { Message = $"Invalid configuration for game {game.Id}. Only {game.Selections.Count} selections but {numbersToDraw} numbers to be drawn" }]
            };
        }

        // Pick the winning game selections
        var winningSelections = GetWinningSelections(game, winningNumbers, numbersToDraw);
        var winningSelectionNumbers = winningSelections.Select(s => s.SelectionNumber);
        _logger.LogInformation("Results for game {gameId} are [{winningNumbers}]", game.Id, string.Join(',', winningSelectionNumbers));


        var serviceUserId = await _repository.GetServiceUserId();

        // Store the winning numbers as game results
        // Only do this if there are not yet any results. 
        // This allows us to re-process anything where an error previously
        // occurred between drawing the numbers but not assigning prizes to the winners.
        if (game.Results.Count == 0)
        {
            game.Results = winningSelections.Select(s => new GameResult
            {
                GameId = game.Id,
                SelectionId = s.Id,
                CreatedById = serviceUserId,
            }).ToList();

            // Save the results at this point
            await _repository.SaveChangesAsync();
        }

        await AssignPrizes(game.Id, winningSelections, serviceUserId);

        game.ResultedAt = timeProvider.GetUtcNow().UtcDateTime;

        await _repository.SaveChangesAsync();

        return new Result<Game>
        {
            Status = ResultStatus.Ok,
            Value = game
        };
    }

    private static IEnumerable<GameSelection> GetWinningSelections(Game game, IEnumerable<int> winningNumbers, int numbersToDraw)
    {
        return winningNumbers.Any()
            ? game.Selections.Where(gs => winningNumbers.Contains(gs.SelectionNumber))
            : Rng.TakeRandom(game.Selections, numbersToDraw);
    }

    private async Task AssignPrizes(Guid gameId, IEnumerable<GameSelection> winningSelections, Guid serviceUserId)
    {
        // Determine the entries who have won prizes.
        // We'll get a list of back with an item for each prize, along with the 
        // entries who have won that prize (if any)
        var prizeWinners = await _repository.GetPrizeWinners(gameId, winningSelections.Select(s => s.Id));

        var entryPrizes = new List<EntryPrize>();
        // Cycle through the prizes and process the winners
        foreach (var (gamePrize, winningEntries) in prizeWinners)
        {
            // If there's no winners for this prize, move on
            if (!winningEntries.Any())
            {
                _logger.LogInformation(
                    "No winners for prize {prize} in game {game}",
                    gamePrize.Position,
                    gamePrize.GameId);

                continue;
            }

            _logger.LogInformation(
                "{count} winner(s) for prize {prize} in game {game}: [{entries}]",
                winningEntries.Count(),
                gamePrize.Position,
                gamePrize.GameId,
                winningEntries.Select(e => $"player {e.CreatedById} entry {e.Id}"));

            // Construct the entry prizes that link a game prize to a player
            entryPrizes.AddRange(winningEntries.Select(we => new EntryPrize
            {
                GamePrizeId = gamePrize.Id,
                EntryId = we.Id,
                CreatedById = serviceUserId
            }));
        }

        // Store the entry prizes
        await _repository.AddEntryPrizes(entryPrizes);
    }
}