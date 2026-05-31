using Lottery.DB.Entities.Idt;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Context;

/// <summary>
/// A context that should be used only for maintenance tasks.
/// 
/// This will take care of seeding data, allowing the standard DB context
/// to not require any knowledge of seed user passwords.
/// </summary>
internal class MaintenanceDBContext(DbContextOptions options) : LotteryDBContext(options)
{
    private readonly PasswordHasher<AppUser> _hasher = new();

    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    //     base.OnModelCreating(builder);

    //     Seeding.ReferenceData.Seed(builder);

    //     Seeding.UsersAndRoles.Seed(builder, _hasher);
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSeeding((context, _) =>
        {
            // Seeding.ReferenceData.Seed(builder);

            Seeding.UsersAndRoles.Seed(context, _hasher);
        });
    }
}