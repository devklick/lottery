using Lottery.DB.Context;
using Lottery.DB.Entities.Dbo;
using Lottery.DB.Repository;
using Lottery.ResultService.Models;

using Microsoft.EntityFrameworkCore;

namespace Lottery.ResultService.Repositories;

public class ResultRepository(LotteryDBContext db) : RepositoryBase<LotteryDBContext>(db)
{
    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();

    public async Task<List<Game>> GetGamesToResult()
    {
        return await _db.Games
            .Where(g => g.State == DB.Entities.Ref.ItemState.Enabled
                && g.DrawTime <= DateTime.UtcNow
                && !_db.GameResults.Any(gr => gr.GameId == g.Id)
            )
            .Include(g => g.Selections)
            .Include(g => g.Prizes)
            .ToListAsync();
    }

    public async Task AddGameResults(IEnumerable<GameResult> winningSelections)
        => await _db.GameResults.AddRangeAsync(winningSelections);

    public async Task<List<(GamePrize Prize, IEnumerable<Entry> WinningEntries)>> GetPrizeWinners(Guid gameId, IEnumerable<Guid> winningGameSelectionIds)
    {
        // Grab the prizes that are available to be won in this game
        var prizes = await _db.GamePrizes
            .Where(gp => gp.GameId == gameId)
            .ToListAsync();

        // Grab the entries and their selections that have 
        // got at least one number correct
        var winners = await _db.EntrySelections
            .Where(es => winningGameSelectionIds.Contains(es.GameSelectionId))
            .Include(x => x.Entry)
            .Include(x => x.Entry.CreatedBy)
            .GroupBy(es => es.EntryId)
            .Select(x => new { EntryId = x.Key, WinningSelections = x.ToList() })
            .ToListAsync();

        var result = new List<(GamePrize Prize, IEnumerable<Entry> WinningEntries)>();

        // Work through the prizes from first to last (smallest number 1 = largest prize)
        foreach (var prize in prizes.OrderBy(r => r.Position))
        {
            // Find the entries that won this prize, if any. 
            // Since entries that have won prize 1 cannot also win prize 2, 
            // we need to be sure to exclude them if they've already been allocated a prize
            var winningEntries = winners
                .Where(we => we.WinningSelections.Count == prize.NumberMatchCount)
                .Where(we => !result.Any(r => r.WinningEntries.Any(e => e.Id == we.EntryId)))
                .SelectMany(we => we.WinningSelections.Select(ws => ws.Entry)).Distinct().ToList();

            result.Add((prize, winningEntries));
        }

        return result;
    }

    public async Task AddEntryPrizes(IEnumerable<EntryPrize> entryPrizes)
        => await _db.EntryPrizes.AddRangeAsync(entryPrizes);
}