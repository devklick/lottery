using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.Game.Result;

public class ResultGameRequestRoute
{
    [Required]
    public required Guid GameId { get; set; }
}