using System.Text.Json.Serialization;

namespace Lottery.Common.Models;

public class Message
{
    public MessageCode Code { get; set; }
    public required string Value { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MessageCode
{
    General,
    RecentAuthRequired,
    EmailVerificationRequired,
    MaxEntriesReached
}