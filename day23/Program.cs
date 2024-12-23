
Solve("test.txt", 3);
//Solve("input.txt", 3);

void Solve(string file, int size)
{
    var conns = ReadConnections(file);
    var sets = BuildSets(conns).ToArray();
    var subSets = BuildSubsets(sets, size)
        .Where(s => s.Any(v => v[0] == 't'))
        .ToArray();

    Console.WriteLine("Sets:");
    foreach (var set in sets)
    {
        Console.WriteLine(string.Join(", ", set));
    }

    Console.WriteLine("\nSubsets:");
    foreach (var set in subSets)
    {
        Console.WriteLine(string.Join(", ", set));
    }

    Console.WriteLine("\nSets found: {0}", subSets.Length);
    Console.WriteLine("Subsets found: {0}", subSets.Length);
}

IEnumerable<HashSet<string>> BuildSubsets(IEnumerable<HashSet<string>> sets, int size)
{
    foreach (var set in sets.Where(s => s.Count >= size))
    {
        foreach (var combo in GenerateCombos([..set], size))
        {
            yield return combo;
        }
    }
}

IEnumerable<HashSet<string>> GenerateCombos(string[] vals, int size)
{
    var ptrs = Enumerable.Range(0, size).ToArray();
    do
    {
        yield return ptrs.Select(i => vals[i]).ToHashSet();
    } while (Increment(ptrs, vals.Length));
}

bool Increment(int[] ptrs, int max)
{
    var i = ptrs.Length - 1;
    while (i >= 0 && ptrs[i] == max - ptrs.Length + i) i--;
    if (i < 0) return false;
    ptrs[i]++;
    while (++i < ptrs.Length) ptrs[i] = ptrs[i - 1] + 1;
    return true;
}

IEnumerable<HashSet<string>> BuildSets(Dictionary<string, HashSet<string>> conns)
{
    SortedSet<string> nodes = [..conns.Keys.OrderByDescending(k => conns[k].Count)];
    while (nodes.Count > 0)
    {
        var node = nodes.First();
        var set = BuildSet(node, conns);
        yield return set;
        nodes.ExceptWith(set);
    }
}

HashSet<string> BuildSet(string seed, Dictionary<string, HashSet<string>> conns)
{
    HashSet<string> set = [seed];
    foreach (var (k, v) in conns.Where(p => p.Key != seed))
    {
        if (v.IsSupersetOf(set))
        {
            set.Add(k);
        }
    }
    return set;
}

Dictionary<string, HashSet<string>> ReadConnections(string file)
{
    var lines = File.ReadAllLines(file)
        .Select(l => l.Split('-'));

    Dictionary<string, HashSet<string>> dict = [];
    foreach (var line in lines)
    {
        void Update(string key, string value)
        {
            if (!dict.TryGetValue(key, out var hash))
            {
                dict[key] = hash = [];
            }

            hash.Add(value);
        }
        Update(line[0], line[1]);
        Update(line[1], line[0]);
    }
    return dict;
}
