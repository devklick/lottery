namespace Lottery.Common.Extensions;

public static class EnumerableExtensions
{
    public static bool HasDuplicate<T>(
        this IEnumerable<T> source,
        IEqualityComparer<T> comparer,
        out T firstDuplicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(comparer);

        (bool result, firstDuplicate) = HasDuplicateImplementation(source, comparer);

        return result;
    }

    public static bool HasDuplicate<T>(
        this IEnumerable<T> source,
        out T firstDuplicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        var comparer = EqualityComparer<T>.Default;

        (bool result, firstDuplicate) = HasDuplicateImplementation(source, comparer);

        return result;
    }

    private static (bool, T) HasDuplicateImplementation<T>(
        IEnumerable<T> source,
        IEqualityComparer<T> comparer)
    {
        var checkBuffer = new HashSet<T>(comparer);
        foreach (var t in source)
        {
            if (!checkBuffer.Add(t))
            {
                return (true, t);
            }
        }

        return (false, default!);
    }

    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> values)
    {
        List<List<T>> result = [];

        for (int count = 0; count < (1 << values.Count()); ++count)
        {
            List<T> combinationList = [];

            for (int i = 0; i < values.Count(); ++i)
            {
                if ((count & (1 << i)) == 0)
                {
                    combinationList.Add(values.ElementAt(i));
                }
            }
            result.Add(combinationList);
        }

        return result;
    }
}