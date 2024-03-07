using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Lottery.DB.Entities.Idt;

[Table(nameof(AppRoleClaim), Schema = nameof(Idt))]
public class AppRoleClaim : IdentityRoleClaim<Guid>
{

}