namespace Lottery.Api.Models.GamePrize.Create;

public class CreateGamePrizeResponse
{
    public Guid Id { get; set; }
    public required int Position { get; set; }

    public required int NumberMatchCount { get; set; }
}