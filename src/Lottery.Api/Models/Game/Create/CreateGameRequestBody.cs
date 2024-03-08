using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Lottery.Api.Models.GamePrize.Create;
using Lottery.Api.Models.GameSelection.Create;
using Lottery.Api.Models.Validation;
using Lottery.DB.Entities.Ref;

namespace Lottery.Api.Models.Game.Create;

public class CreateGameRequestBody
{
    [Required]
    public required DateTime StartTime { get; set; }

    [Required]
    public required DateTime DrawTime { get; set; }

    [Required, StringLength(64)]
    public required string Name { get; set; }

    [DefaultValue(ItemState.Enabled)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ItemState State { get; set; }

    [Required]
    public required int NumbersRequired { get; set; }


    [Required, UniqueValues<CreateGameSelectionRequestBody>]
    public required List<CreateGameSelectionRequestBody> Selections { get; set; }

    [Required]
    public required List<CreateGamePrizeRequestBody> Prizes { get; set; }
}