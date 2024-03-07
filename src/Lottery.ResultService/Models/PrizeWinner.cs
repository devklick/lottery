using Lottery.DB.Entities.Dbo;

namespace Lottery.ResultService.Models;

public class PrizeWinner
{
    public required GamePrize Prize { get; set; }
    public required List<Entry> WinningEntries { get; set; }
}