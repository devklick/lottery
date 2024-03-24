namespace Lottery.Api.Models.GameResult.Get;

public class GetGameResultResponse
{
    public Guid Id { get; set; }
    public Guid SelectionId { get; set; }
    public int SelectionNumber { get; set; }
}