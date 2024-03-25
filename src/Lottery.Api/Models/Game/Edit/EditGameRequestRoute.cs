using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.Game.Edit;

public class EditGameRequestRoute
{

    [Required]
    public Guid Id { get; set; }
}