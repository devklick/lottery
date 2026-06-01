namespace Lottery.Integration.Test.Abstractions;

public class FakeTimeProvider : TimeProvider
{
    public DateTimeOffset UtcNow { get; set; }

    public override DateTimeOffset GetUtcNow() => UtcNow;
}