using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Entities.Dbo;

using Microsoft.AspNetCore.Identity;

namespace Lottery.DB.Entities.Idt;

[Table(nameof(AppUser), Schema = nameof(Idt))]
public class AppUser : IdentityUser<Guid>
{
    public List<Entry> Entries { get; set; } = [];
}