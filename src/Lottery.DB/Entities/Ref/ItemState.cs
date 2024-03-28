using System.Text.Json.Serialization;

namespace Lottery.DB.Entities.Ref;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemState
{
    Enabled = 1,
    Disabled = 0
}