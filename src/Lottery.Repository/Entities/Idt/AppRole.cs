using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Lottery.Repository.Entities.Idt;

[Table(nameof(AppRole), Schema = nameof(Idt))]
public class AppRole : IdentityRole<Guid>
{
    [Required, StringLength(64)]
    public required string DisplayName { get; set; }

    [Required, StringLength(64)]
    public required string Description { get; set; }
}