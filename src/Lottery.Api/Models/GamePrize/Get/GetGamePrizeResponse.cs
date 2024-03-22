namespace Lottery.Api.Models.GamePrize.Get;

public class GetGamePrizeResponse
{
    public required Guid Id { get; set; }
    public required int Position { get; set; }
    public required int NumberMatchCount { get; set; }

}