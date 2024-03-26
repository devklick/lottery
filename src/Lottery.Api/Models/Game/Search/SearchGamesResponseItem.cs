using Lottery.DB.Entities.Dbo;

namespace Lottery.Api.Models.Game.Search;

public class SearchGamesResponseItem
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime CloseTime { get; set; }
    public required DateTime DrawTime { get; set; }
    public required int SelectionsRequiredForEntry { get; set; }
    public required GameStatus GameState { get; set; }
    public required List<Selection> Selections { get; set; }
    public required List<Result> Results { get; set; }
    public required List<Prize> Prizes { get; set; }

    public class Selection
    {
        public required Guid Id { get; set; }
        public required int SelectionNumber { get; set; }
    }

    public class Result
    {
        public Guid Id { get; set; }
        public Guid SelectionId { get; set; }
        public int SelectionNumber { get; set; }
    }

    public class Prize
    {
        public required Guid Id { get; set; }
        public required int Position { get; set; }
        public required int NumberMatchCount { get; set; }
    }
}