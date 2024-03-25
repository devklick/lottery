using Lottery.Api.Models.GamePrize.Create;
using Lottery.Api.Models.GameSelection.Create;

namespace Lottery.Api.Models.Game.Create;

public class CreateGameResponse
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime CloseTime { get; set; }

    public DateTime DrawTime { get; set; }

    public required string Name { get; set; }

    public required int SelectionsRequiredForEntry { get; set; }

    public required List<CreateGameSelectionResponse> Selections { get; set; }
    public required List<CreateGamePrizeResponse> Prizes { get; set; }
}