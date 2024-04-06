using Lottery.DB.Context;

using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Repositories;

public abstract class RepositoryBase<TContext>(TContext db) where TContext : LotteryDBContext
{
    protected readonly TContext _db = db;

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();

    /// <summary>
    /// Returns the <see cref="Entities.Idt.AppUser"/> who's <see cref="Entities.Idt.AppUser.UserName"/>
    /// matches the username of the current database connection.
    /// </summary>
    /// <exception cref="Exception">Throws if no AppUser exists with the connection username</exception>
    public async Task<Guid> GetServiceUserId()
        => await _db.Database.SqlQueryRaw<Guid?>(
            "SELECT u.id \"Value\" FROM idt.app_user u WHERE u.user_name = current_user")
            .SingleOrDefaultAsync()
            ?? throw new Exception("No app user matching DB user name");
}