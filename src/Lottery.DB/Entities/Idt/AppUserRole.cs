using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Lottery.DB.Entities.Idt;

[Table(nameof(AppUserRole), Schema = nameof(Idt))]
public class AppUserRole : IdentityUserRole<Guid>
{

}