using System.Diagnostics.CodeAnalysis;

namespace Lottery.Common.Models;

public class Result<TValue>
{
    public TValue? Value { get; set; }
    public ResultStatus Status { get; set; }
    public List<Message>? Messages { get; set; }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Messages))]
    public bool Success => Status == ResultStatus.Ok;

    /// <summary>
    /// Creates a new <see cref="Result{TValue}"/> object where the <see cref="Value"/> is the specified <paramref name="value"/>.
    /// The <see cref="Status"/> and <see cref="Messages"/> are copied from the existing instance.
    /// </summary>
    public Result<TOther> ChangeValue<TOther>(TOther? value = default)
        => new() { Messages = Messages, Status = Status, Value = value };

    public static Result<TValue> Error(ResultStatus status, params IEnumerable<string> messages)
        => Error(status, messages.Select(m => new Message { Value = m, Code = MessageCode.General }));

    public static Result<TValue> Error(ResultStatus status, params IEnumerable<Message> messages)
        => new() { Messages = [.. messages], Status = status };

    public static Result<TValue> Ok(TValue value) => new() { Value = value, Status = ResultStatus.Ok };

    public void AddMessages(params string[] messages)
    {
        Messages ??= [];
        Messages.AddRange(messages.Select(e => new Message { Value = e }));
    }
    public void AddMessages(params Message[] messages)
    {
        Messages ??= [];
        Messages.AddRange(messages);
    }

    public void AddMessages(MessageCode code, params string[] messages)
    {
        Messages ??= [];
        Messages.AddRange(messages.Select(m => new Message { Value = m, Code = code }));
    }
}