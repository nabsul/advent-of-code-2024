var input = "0 7 6618216 26481 885 42 202642 8791".Split(' ').Select(long.Parse).ToArray();

var memo = new Dictionary<int, Dictionary<long, IEnumerable<long>>>();
Enumerable.Range(0, 75+1).ToList().ForEach(n => memo.Add(n, []));

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

IEnumerable<long> SingleValueMemo(long i)
{
    if (i == 0) return [1];
    var m = memo[0];
    if (!m.TryGetValue(i, out var values))
    {
        values = SingleValue(i).ToArray();
        m.Add(i, values);
    }

    return values;
}

IEnumerable<long> IterateSingle(long val, int n)
{
    if (n == 0) return [val];

    var m = memo[n];
    if (!m.TryGetValue(val, out var values))
    {
        values = IterateSingle(val, n - 1).SelectMany(SingleValueMemo).ToArray();
        m.Add(val, values);
    }

    return values;
}

IEnumerable<long> Iterate(IEnumerable<long> arr, int n)
{
    if (n == 0) return arr;
    return arr.SelectMany(i => IterateSingle(i, n));
}

var part1 = Iterate(input, 25).Count();
Console.WriteLine($"Part 1: {part1}");

var part2 = Iterate(input, 75).Count();
Console.WriteLine($"Part 2: {part2}");
