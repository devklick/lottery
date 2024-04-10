using System.Diagnostics.CodeAnalysis;

namespace Lottery.Common.Comparers;

public class UnorderedListComparer<T> : IEqualityComparer<IEnumerable<T>>
{
    public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
    {
        return x != null && y != null && Enumerable.SequenceEqual(
            x.OrderBy(t => t?.GetHashCode()),
            y.OrderBy(t => t?.GetHashCode())
        );
    }

    public int GetHashCode([DisallowNull] IEnumerable<T> obj)
    {
        unchecked
        {
            if (obj == null) return 0;
            const int seedValue = 0x2D2816FE;
            const int primeNumber = 397;
            return obj.OrderBy(a => a).Aggregate(seedValue, (current, item) => (current * primeNumber) + (Equals(item, default(T)) ? 0 : item?.GetHashCode() ?? 0));
        }
    }
}