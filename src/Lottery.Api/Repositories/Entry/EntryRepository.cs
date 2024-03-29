using Lottery.Api.Repositories.Entry.Filters;
using Lottery.DB.Context;
using Lottery.DB.Entities.Dbo;
using Lottery.DB.Repositories;

using Microsoft.EntityFrameworkCore;

using EntryEntity = Lottery.DB.Entities.Dbo.Entry;

namespace Lottery.Api.Repositories.Entry;

public class EntryRepository(LotteryDBContext db) : RepositoryBase<LotteryDBContext>(db)
{
    public async Task<EntryEntity> CreateEntry(EntryEntity entry)
    {
        var result = await _db.Entries.AddAsync(entry);
        await _db.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<EntryEntity?> GetEntry(Guid entryId, GetEntry.GameFilter? gamesFilter = null, GetEntry.SelectionsFilter? selectionsFilter = null)
    {
        var query = _db.Entries.Where(s => s.Id == entryId);
        if ((gamesFilter?.Include) ?? false)
        {
            query = query.Include(e => e.Game);

            if (gamesFilter.SelectionFilter.Include)
            {
                query = query.Include(e => e.Game).ThenInclude(g => g.Selections.Where(s =>
                    !gamesFilter.SelectionFilter.State.HasValue || s.State == gamesFilter.SelectionFilter.State));
            }
        }

        if ((selectionsFilter?.Include) ?? false)
        {
            query = query.Include(e => e.Selections.Where(s =>
            !selectionsFilter.State.HasValue || s.State == selectionsFilter.State));
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<(IEnumerable<EntryEntity> Entries, int Total)> SearchEntries(Guid userId, Guid? gameId, int page, int limit)
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

    public async Task<EntryEntity> UpdateEntry(EntryEntity entry)
    {
        _db.Update(entry);

        await _db.SaveChangesAsync();

        return entry;
    }
}