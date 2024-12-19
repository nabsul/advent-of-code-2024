using System.Text.Json;

CountPossible("test.txt");
CountPossible("input.txt");

void CountPossible(string file)
{
    var (towels, patterns) = ReadInput(file);
    long count = 0;
    long combos = 0;
    foreach (var p in patterns)
    {
        var c = CountCombos(towels, p, []);
        combos += c;
        count += c > 0 ? 1 : 0;
    }
    Console.WriteLine($"Number of possible patterns: {count} and {combos} combinations");
}

long CountCombos(string[] towels, string pattern, Dictionary<string, long> memo)
{
    if (memo.TryGetValue(pattern, out var res)) return res;
    long count = 0;
    foreach (var t in towels)
    {
        if (pattern == t) count++;
        
        if (pattern.StartsWith(t))
        {
            count += CountCombos(towels, pattern[t.Length..], memo);
        }
    }
    memo[pattern] = count;
    return count;
}

(string[] towels, string[] patterns)  ReadInput(string file)
{
    var lines = File.ReadAllLines(file);
    var towels = lines[0].Split(", ");
    var patterns = lines.Skip(2);
    return (towels, patterns.ToArray());
}
