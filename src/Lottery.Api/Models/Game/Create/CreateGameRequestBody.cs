using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Lottery.Api.Models.GameSelection.Create;
using Lottery.Api.Models.Validation;
using Lottery.Repository.Entities.Ref;

namespace Lottery.Api.Models.Game.Create;

public class CreateGameRequestBody
{
    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime DrawTime { get; set; }

    [Required, StringLength(64)]
    public required string Name { get; set; }

    [DefaultValue(ItemState.Enabled)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ItemState State { get; set; }

    [Required, UniqueValues<CreateGameSelectionRequestBody>]
    public required List<CreateGameSelectionRequestBody> Selections { get; set; }
}