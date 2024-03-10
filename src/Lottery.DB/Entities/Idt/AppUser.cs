using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Attributes;
using Lottery.DB.Entities.Dbo;
using Lottery.DB.Entities.Ref;

using Microsoft.AspNetCore.Identity;

namespace Lottery.DB.Entities.Idt;

[Table(nameof(AppUser), Schema = nameof(Idt))]
public class AppUser : IdentityUser<Guid>
{
    public List<Entry> Entries { get; set; } = [];

    [Required, SqlColumnDefaultConstraint(AccountType.User)]
    public AccountType AccountType { get; set; } = AccountType.User;
}