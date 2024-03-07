using System.ComponentModel.DataAnnotations;

namespace Lottery.Api.Models.GamePrize.Create;

public class CreateGamePrizeRequestBody
{
    [Required]
    public int Position { get; set; }

    [Required]
    public int NumberMatchCount { get; set; }
}