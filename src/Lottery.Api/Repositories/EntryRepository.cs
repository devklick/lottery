using Lottery.DB.Context;
using Lottery.DB.Entities.Dbo;
using Lottery.DB.Repository;

using Microsoft.EntityFrameworkCore;

namespace Lottery.Api.Repositories;

public class EntryRepository(LotteryDBContext db) : RepositoryBase<LotteryDBContext>(db)
{
    public async Task<Entry> CreateEntry(Entry entry)
    {
        var result = await _db.Entries.AddAsync(entry);
        await _db.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<(IEnumerable<Entry> Entries, int Total)> SearchEntries(Guid userId, Guid? gameId, int page, int limit)
    {
        var query = _db.Entries
            .Include(entry => entry.Prize)
            .ThenInclude(prize => prize.GamePrize)
            .Include(entry => entry.Selections)
            .ThenInclude(selection => selection.GameSelection)
            .Where(e => e.CreatedById == userId)
            .AsQueryable();

        if (gameId.HasValue)
        {
            query = query.Where(e => e.GameId == gameId);
        }

        query = query.OrderByDescending(e => e.CreatedOnUtc);

        var total = await query.CountAsync();

        var entries = await query.Skip((page - 1) * limit)
            .Take(limit).ToListAsync();

        return (entries, total);
    }

}