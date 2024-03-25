using Lottery.DB.Entities.Dbo;

namespace Lottery.Api.Models.Game.Edit;

public class EditGameResponse
{
    public required Guid Id { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime CloseTime { get; set; }
    public required DateTime DrawTime { get; set; }
    public required DateTime? ResultedAt { get; set; }
    public required string Name { get; set; }
    public required int SelectionsRequiredForEntry { get; set; }
    public required GameStatus GameStatus { get; set; }
    public List<EditGameResponse_GameSelection> Selections { get; set; } = [];
    public List<EditGameResponse_GamePrize> Prizes { get; set; } = [];
    public List<EditGameResponse_GameResult> Results { get; set; } = [];
}

public class EditGameResponse_GameSelection
{
    public required Guid Id { get; set; }
    public required int SelectionNumber { get; set; }
}

public class EditGameResponse_GamePrize
{
    public required Guid Id { get; set; }
    public required int Position { get; set; }
    public required int NumberMatchCount { get; set; }
}

public class EditGameResponse_GameResult
{
    public Guid Id { get; set; }
    public Guid SelectionId { get; set; }
    public int SelectionNumber { get; set; }
}