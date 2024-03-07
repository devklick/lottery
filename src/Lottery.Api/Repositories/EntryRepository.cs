using Lottery.DB.Context;
using Lottery.DB.Entities.Dbo;

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
}