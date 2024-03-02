using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Lottery.Common.Extensions;

public static class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) { return input; }

        var startUnderscores = Regex.Match(input, @"^_+");

        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }

    public static string UrlEncode(this string value, Encoding? encoding = null)
        => HttpUtility.UrlEncode(value, encoding ?? Encoding.UTF8);

    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value)
        => string.IsNullOrEmpty(value);

    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? value)
        => string.IsNullOrWhiteSpace(value);

    public static string TrimEnd(this string value, string valueToRemove)
    {
        if (value.Length >= valueToRemove.Length && value.Substring(value.Length - valueToRemove.Length, valueToRemove.Length) == valueToRemove)
        {
            return value.Substring(0, value.Length - valueToRemove.Length);
        }
        return value;
    }

    public static string RemoveAccent(this string txt)
    {
        byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
        return Encoding.ASCII.GetString(bytes);
    }

    public static string Slugify(this string phrase)
    {
        string str = phrase.RemoveAccent().ToLower();
        str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // Remove all non valid chars          
        str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space  
        str = Regex.Replace(str, @"\s", "-"); // //Replace spaces by dashes
        return str;
    }
}