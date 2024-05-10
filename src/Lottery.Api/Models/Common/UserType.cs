using System.Text.Json.Serialization;

namespace Lottery.Api.Models.Common;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserType
{
    /// <summary>
    /// Users who are not authenticated are assigned the <see cref="Guest"/> user type
    /// </summary>
    Guest,

    /// <summary>
    /// General authenticated users are assigned the <see cref="Basic"/> user type
    /// </summary>
    Basic,

    /// <summary>
    /// Users who have access to manage games are assigned the <see cref="Admin"/> user type
    /// </summary>
    Admin,

    /// <summary>
    /// Users who have access to manage the entire platform are assigned <see cref="SystemAdmin"/> user type
    /// </summary>
    SystemAdmin
}