using System.Text.Json.Serialization;

namespace Lottery.Api.Models.Common;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    Asc,
    Desc
}