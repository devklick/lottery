using System.ComponentModel.DataAnnotations;
using Lottery.Api.Models.Validation;

namespace Lottery.Api.Models;

public class CreateGameRequestBody
{
    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime DrawTime { get; set; }

    [Required, StringLength(64)]
    public required string Name { get; set; }

    [Required]
    public required List<CreateGameRequestBody_Selection> Selections { get; set; }
}

public class CreateGameRequestBody_Selection
{
    [Required, UniqueValues]
    public int SelectionNumber { get; set; }
}