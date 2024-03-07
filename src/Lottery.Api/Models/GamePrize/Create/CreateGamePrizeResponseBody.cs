namespace Lottery.Api.Models.GamePrize.Create;

public class CreateGamePrizeResponseBody
{
    public required int Position { get; set; }

    public required int NumberMatchCount { get; set; }
}