using Lottery.DB.Context;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Repositories;

public abstract class RepositoryBase<TContext>(TContext db) where TContext : LotteryDBContext
{
    protected readonly TContext _db = db;

    /// <summary>
    /// Returns the <see cref="Entities.Idt.AppUser"/> who's <see cref="Entities.Idt.AppUser.UserName"/>
    /// matches the username of the current database connection.
    /// </summary>
    /// <exception cref="Exception">Throws if no AppUser exists with the connection username</exception>
    public async Task<Guid> GetServiceUserId()
        => await _db.Database.SqlQueryRaw<Guid?>(
            "SELECT u.id FROM idt.app_user u WHERE u.username = current_user;")
            .SingleOrDefaultAsync()
            ?? throw new Exception("No app user matching DB user name");
}