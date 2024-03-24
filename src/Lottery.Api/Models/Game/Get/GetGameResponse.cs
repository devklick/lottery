using Lottery.Api.Models.GamePrize.Get;
using Lottery.Api.Models.GameResult.Get;
using Lottery.Api.Models.GameSelection.Get;
using Lottery.DB.Entities.Dbo;

namespace Lottery.Api.Models.Game.Get;

public class GetGameResponse
{
    public required Guid Id { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime DrawTime { get; set; }
    public required DateTime? ResultedAt { get; set; }
    public required string Name { get; set; }
    public required int SelectionsRequiredForEntry { get; set; }
    public required GameStatus GameStatus { get; set; }
    public List<GetGameSelectionResponse> Selections { get; set; } = [];
    public List<GetGamePrizeResponse> Prizes { get; set; } = [];
    public List<GetGameResultResponse> Results { get; set; } = [];
}