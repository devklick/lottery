using Lottery.DB.Entities.Dbo;
using Lottery.DB.Entities.Ref;

namespace Lottery.Api.Models.Game.Get;

public class GetGameResponse
{
    public required Guid Id { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime CloseTime { get; set; }
    public required DateTime DrawTime { get; set; }
    public required DateTime? ResultedAt { get; set; }
    public required string Name { get; set; }
    public required int SelectionsRequiredForEntry { get; set; }
    public required GameStatus GameStatus { get; set; }
    public required ItemState State { get; set; }
    public List<Selection> Selections { get; set; } = [];
    public List<Prize> Prizes { get; set; } = [];
    public List<Result> Results { get; set; } = [];

    public class Selection
    {
        public required Guid Id { get; set; }
        public required int SelectionNumber { get; set; }
    }

    public class Prize
    {
        public required Guid Id { get; set; }
        public required int Position { get; set; }
        public required int NumberMatchCount { get; set; }
    }

    public class Result
    {
        public Guid Id { get; set; }
        public Guid SelectionId { get; set; }
        public int SelectionNumber { get; set; }
    }
}