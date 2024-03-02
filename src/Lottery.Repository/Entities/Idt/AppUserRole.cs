using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lottery.Repository.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lottery.Repository.Entities.Idt;

[Table(nameof(AppUserRole), Schema = nameof(Idt))]
public class AppUserRole : IdentityUserRole<Guid>
{

}