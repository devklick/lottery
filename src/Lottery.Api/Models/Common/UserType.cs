using System.Text.Json.Serialization;

namespace Lottery.Api.Models.Common;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserType
{
    Guest,
    Basic,
    Admin
}