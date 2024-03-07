using Lottery.Api.Models.GamePrize.Create;
using Lottery.Api.Models.GameSelection.Create;

namespace Lottery.Api.Models.Game.Create;

public class CreateGameResponseBody
{
    public DateTime StartTime { get; set; }

    public DateTime DrawTime { get; set; }

    public required string Name { get; set; }

    public required int NumbersToDraw { get; set; }

    public required List<CreateGameSelectionResponseBody> Selections { get; set; }
    public required List<CreateGamePrizeResponseBody> Prizes { get; set; }
}