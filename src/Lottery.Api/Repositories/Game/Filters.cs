using Lottery.DB.Entities.Ref;

namespace Lottery.Api.Repositories.Game.Filters;

public class GetGame_SelectionsFilter
{
    public bool Include { get; set; }
    public ItemState? State { get; set; }
}
public class GetGame_PrizesFilter
{
    public bool Include { get; set; }
    public ItemState? State { get; set; }
}
public class GetGame_ResultsFilter
{
    public bool Include { get; set; }
    public ItemState? State { get; set; }
}