using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Lottery.Repository.Entities.Idt;

[Table(nameof(AppUserClaim), Schema = nameof(Idt))]
public class AppUserClaim : IdentityUserClaim<Guid>
{

}