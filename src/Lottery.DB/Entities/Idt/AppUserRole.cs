using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Lottery.DB.Attributes;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lottery.DB.Entities.Idt;

[Table(nameof(AppUserRole), Schema = nameof(Idt))]
public class AppUserRole : IdentityUserRole<Guid>
{

}