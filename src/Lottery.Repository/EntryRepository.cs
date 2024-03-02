using Lottery.Repository.Context;
using Lottery.Repository.Entities.Dbo;

namespace Lottery.Repository;

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