namespace Lottery.ResultService.Services;

public class Rng()
{
    public static IEnumerable<T> TakeRandom<T>(IEnumerable<T> items, int count)
    {
        var set = new List<T>(items);
        var results = new List<T>(count);

        for (var i = 0; i < count; i++)
        {
            var index = Random.Shared.Next(0, set.Count - 1);
            results.Add(set[index]);
            set.RemoveAt(index);
        }

        return results;
    }
}