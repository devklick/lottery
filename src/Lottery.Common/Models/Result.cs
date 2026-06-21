using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;


namespace Lottery.Common.Models;

// TODO: Rename Errors property to Messages
// Useful to include non-error messages back to the client

public class Result<TValue>
{
    public TValue? Value { get; set; }
    public ResultStatus Status { get; set; }
    public List<Error>? Errors { get; set; }

    [JsonIgnore]
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Errors))]
    public bool IsOk => Status == ResultStatus.Ok;

    /// <summary>
    /// Creates a new <see cref="Result{TValue}"/> object where the <see cref="Value"/> is the specified <paramref name="value"/>.
    /// The <see cref="Status"/> and <see cref="Errors"/> are copied from the existing instance.
    /// </summary>
    public Result<TOther> ChangeValue<TOther>(TOther? value = default)
        => new() { Errors = Errors, Status = Status, Value = value };

    public static Result<TValue> Error(ResultStatus status, params IEnumerable<string> errors)
        => new() { Errors = [.. errors.Select(e => new Error() { Message = e })], Status = status };

    public static Result<TValue> FromErrorResult<T>(Result<T> errorResult)
        => new() { Errors = errorResult.Errors, Status = errorResult.Status };

    public static Result<TValue> Ok(TValue value) => new() { Value = value, Status = ResultStatus.Ok };

    public void AddErrors(params string[] errors)
    {
        Errors ??= [];
        Errors.AddRange(errors.Select(e => new Error { Message = e }));
    }
}