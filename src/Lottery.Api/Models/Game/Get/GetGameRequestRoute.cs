using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.Game.Get;

public class GetGameRequestRoute
{
    [Required]
    public required Guid Id { get; set; }
}