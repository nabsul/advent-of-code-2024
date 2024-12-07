
long count1 = 0;
long total1 = 0;
long count2 = 0;
long total2 = 0;
long countAll = 0;

foreach (var line in File.ReadAllLines("data.txt"))
{
    var parts = line.Split(": ");
    var target = long.Parse(parts[0]);
    var values = parts[1].Split(" ").Select(long.Parse).ToArray();

    countAll++;
    if (Solve(false, target, values, values.Length - 1))
    {
        count1++;
        total1 += target;
    }

    if (Solve(true, target, values, values.Length - 1))
    {
        count2++;
        total2 += target;
    }
}

Console.WriteLine($"Part1: {total1} ({count1} of {countAll})");
Console.WriteLine($"Part2: {total2} ({count2} of {countAll})");

static bool Solve(bool withOr, long target, long[] values, int idx)
{
    if (target < 0) return false;
    var currValue = values[idx];
    if (idx == 0) return target == currValue;

    if (Solve(withOr, target - currValue, values, idx - 1)) return true;

    if (target % currValue == 0 && Solve(withOr, target / currValue, values, idx - 1)) return true;

    if (withOr)
    {
        var strTarget = target.ToString();
        var strCurrValue = currValue.ToString();
        if (strTarget.Length > strCurrValue.Length && strTarget.EndsWith(strCurrValue))
        {
            Console.WriteLine($"{target} -> {currValue}");
            var newTarg = long.Parse(strTarget[..^strCurrValue.Length]);
            Console.WriteLine($"{target} -> {newTarg} || {currValue}");
            if (Solve(withOr, newTarg, values, idx - 1)) return true;
        }
    }

    return false;
}
