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

    public async Task<(IEnumerable<EntryEntity> Entries, int Total)> SearchEntries(
        int page, int limit,
        SearchEntries.EntryFilter? entryFilter = null,
        SearchEntries.GameFilter? gameFilter = null,
        SearchEntries.PrizeFilter? prizeFilter = null,
        SearchEntries.SelectionsFilter? selectionsFilter = null)
    {
        var query = _db.Entries.AsQueryable();

        if (entryFilter != null)
        {
            query = query.Where(e =>
                (!entryFilter.State.HasValue || entryFilter.State == e.State)
                && (!entryFilter.UserId.HasValue || entryFilter.UserId == e.CreatedById));
        }

        if (gameFilter != null)
        {
            if (gameFilter.Include) query = query.Include(e => e.Game);
            query = query.Where(e =>
                (!gameFilter.GameId.HasValue || e.GameId == gameFilter.GameId)
                && (!gameFilter.State.HasValue || e.Game.State == gameFilter.State));
        }

        if (prizeFilter != null)
        {
            if (prizeFilter.Include) query = query.Include(e => e.Prize).ThenInclude(p => p.GamePrize);
            query = query.Where(e => e.Prize == null || !prizeFilter.State.HasValue || e.Prize.State == prizeFilter.State);
        }

        if (selectionsFilter != null)
        {
            if (selectionsFilter.Include) query = query.Include(e =>
                e.Selections.Where(s => !selectionsFilter.State.HasValue || s.State == selectionsFilter.State))
                .ThenInclude(s => s.GameSelection);
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