namespace Lottery.Api.Models.GameSelection.Get;

public class GetGameSelectionResponse
{
    public required Guid Id { get; set; }
    public required int SelectionNumber { get; set; }
}