namespace Lottery.Common.Models;

public class Result<TValue>
{
    public TValue? Value { get; set; }
    public ResultStatus Status { get; set; }
    public List<Error>? Errors { get; set; }
}