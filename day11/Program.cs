var input = "0 7 6618216 26481 885 42 202642 8791".Split(' ').Select(long.Parse).ToArray();

IEnumerable<long> SingleValue(long i)
{
    if (i == 0) return [1];

    var s = i.ToString();
    if (s.Length % 2 == 1) return [i * 2024];

    var mid = s.Length / 2;
    var left = s[..mid];
    var right = s[mid..];

    return [long.Parse(left), long.Parse(right)];
}

long Iterate(Dictionary<long, long> counts, int n)
{
    if (n == 0)
    {
        return counts.Values.Sum();
    }

    var next = new Dictionary<long, long>();

    foreach (var (k, v) in counts)
    {
        foreach (var i in SingleValue(k))
        {
            if (!next.ContainsKey(i))
            {
                next[i] = 0;
            }

            next[i] += v;
        }
    }

    return Iterate(next, n - 1);
}

var c = input.GroupBy(v => v).ToDictionary(g => g.Key, g => (long)g.Count());

var part1 = Iterate(c, 25);
Console.WriteLine($"Part 1: {part1}");

var part2 = Iterate(c, 75);
Console.WriteLine($"Part 2: {part2}");
