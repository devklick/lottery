using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Lottery.DB.Context;

/// <summary>
/// A context that should be used only for maintenance tasks.
/// 
/// This will take care of seeding data, allowing the standard DB context
/// to not require any knowledge of seed user passwords.
/// </summary>
internal class MaintenanceDBContext(DbContextOptions options, IConfiguration config) : LotteryDBContext(options, config)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        Seeding.ReferenceData.Seed(builder);

        Seeding.UsersAndRoles.Seed(builder);
    }
}