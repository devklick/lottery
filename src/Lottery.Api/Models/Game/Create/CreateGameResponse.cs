namespace Lottery.Api.Models.Game.Create;

public class CreateGameResponse
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime CloseTime { get; set; }

    public DateTime DrawTime { get; set; }

    public required string Name { get; set; }

    public required int SelectionsRequiredForEntry { get; set; }

    public required List<Selection> Selections { get; set; }
    public required List<Prize> Prizes { get; set; }

    public class Selection
    {
        public Guid Id { get; set; }
        public int SelectionNumber { get; set; }
    }

    public class Prize
    {
        public Guid Id { get; set; }
        public required int Position { get; set; }
        public required int NumberMatchCount { get; set; }
    }
}