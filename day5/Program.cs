var allData = File.ReadAllLines("data.txt");
var splitIdx = allData.ToList().FindIndex(string.IsNullOrWhiteSpace);

var rules = allData[..splitIdx];
var prints = allData[(splitIdx + 1)..];

Console.WriteLine($"Read {rules.Length} rules and {prints.Length} prints");

Dictionary<int, int[]> ruleMap = rules
    .Select(r => r.Split('|'))
    .Select(parts => (int.Parse(parts[0]), int.Parse(parts[1])))
    .GroupBy(p => p.Item1)
    .ToDictionary(
        g => g.Key,
        g => g.Select(p => p.Item2).ToArray()
    );

int validCount = 0;
int validSum = 0;
int fixedCount = 0;
int fixedSum = 0;

foreach (var print in prints)
{
    var order = print.Split(',').Select(int.Parse).ToArray();

    if (isValid(order))
    {
        validCount++;
        validSum += order[order.Length / 2];
    }
    else
    {
        var list = order.ToList();
        fixOrder(list);
        fixedCount++;
        fixedSum += list[order.Length / 2];
    }
}

Console.WriteLine($"Part1 valid: {validCount}");
Console.WriteLine($"Part1 result: {validSum}");
Console.WriteLine($"Part2 fixed: {fixedCount}");
Console.WriteLine($"Part2 result: {fixedSum}");

bool isValid(int[] order)
{
    HashSet<int> seen = [];

    foreach (var i in order)
    {
        if (!ruleMap.TryGetValue(i, out var list))
        {
            continue;
        }

        if (list.Any(j => seen.Contains(j)))
        {
            return false;
        }

        seen.Add(i);
    }

    return true;
}

void fixOrder(List<int> order)
{
    Dictionary<int, int> seen = [];
    var idx = 0;
    while (idx < order.Count)
    {
        var i = order[idx];

        if (!ruleMap.TryGetValue(i, out var list))
        {
            continue;
        }

        var edited = false;
        foreach (var j in list)
        {
            if (seen.TryGetValue(j, out var pos))
            {
                order.RemoveAt(pos);
                order.Insert(idx, j);
                edited = true;
                break;
            }
        }

        if (edited)
        {
            idx = 0;
            seen.Clear();
            continue;
        }

        seen.Add(i, idx);
        idx++;
    }
}
