using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Lottery.DB.Entities.Idt;

[Table(nameof(AppUserLogin), Schema = nameof(Idt))]
public class AppUserLogin : IdentityUserLogin<Guid>
{

}