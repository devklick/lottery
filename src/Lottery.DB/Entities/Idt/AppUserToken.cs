using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Lottery.DB.Entities.Idt;

[Table(nameof(AppUserToken), Schema = nameof(Idt))]
public class AppUserToken : IdentityUserToken<Guid>
{
}