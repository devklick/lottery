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

    public async Task<(IEnumerable<Entry> Entries, int Total)> GetEntries(Guid userId, int page, int limit)
    {
        var query = _db.Entries
            .Where(e => e.CreatedById == userId)
            .OrderByDescending(e => e.CreatedOnUtc);

        var total = await query.CountAsync();
        var entries = await query.Skip((page - 1) * limit)
            .Take(limit).ToListAsync();

        return (entries, total);
    }

}