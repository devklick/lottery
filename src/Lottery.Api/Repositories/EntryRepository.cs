using Lottery.DB.Context;
using Lottery.DB.Entities.Dbo;

using Microsoft.EntityFrameworkCore;

namespace Lottery.Api.Repositories;

public class EntryRepository(LotteryDBContext db)
{
    private readonly LotteryDBContext _db = db;

    public async Task<Entry> CreateEntry(Entry entry)
    {
        var result = await _db.Entries.AddAsync(entry);
        await _db.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<(IEnumerable<Entry> Entries, bool HasMore)> GetEntries(Guid userId, int page, int limit)
    {
        var entries = await _db.Entries
            .Where(e => e.CreatedById == userId)
            .OrderByDescending(e => e.CreatedOnUtc)
            .Skip((page - 1) * limit)
            .Take(limit + 1)
            .ToListAsync();

        return (entries.Take(limit), entries.Count > limit);
    }

}