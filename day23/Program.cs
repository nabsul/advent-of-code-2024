
Solve("test.txt", 3);
Solve("input.txt", 3);

void Solve(string file, int size)
{
    var conns = ReadConnections(file).ToArray();
    var subSets = CollapseSets(conns).ToArray();
    var filtered = subSets.Where(s => s.Any(v => v[0] == 't')).ToArray();

    Console.WriteLine("\nSets found: {0}", subSets.Length);
    Console.WriteLine("Filtered found: {0}", filtered.Length);

    while (subSets.Length > 1)
    {
        subSets = CollapseSets(subSets).ToArray();
    }

    if (subSets.Length == 1)
    {
        Console.WriteLine("Final set: {0}", string.Join(",", subSets[0]));
    }
    else
    {
        Console.WriteLine("No final set found");
    }
}

IEnumerable<string[]> CollapseSets(IEnumerable<string[]> sets)
{
    var list = sets.Select(s => string.Join("-", s)).OrderBy(s => s).ToList();
    var lookup = list.ToHashSet();
    var idx = 0;

    //Console.WriteLine("List: {0}", string.Join(", ", list));

    while (idx < list.Count - 1)
    {
        var idx2 = idx + 1;
        while (idx2 < list.Count && IsPossible(list[idx], list[idx2]))
        {
            var combos = GetCombos(list[idx], list[idx2]).ToArray();
            //Console.WriteLine($"Combos: {string.Join(", ", combos)}");
            if (combos.All(c => lookup.Contains(c)))
            {
                yield return MakeSuperSet(list[idx], list[idx2]);
            }

            idx2++;
        }

        idx++;
    }
}

bool IsPossible(string a, string b)
{
    return a[..^2] == b[..^2];
}

IEnumerable<string> GetCombos(string a, string b)
{
    var parts = MakeSuperSet(a, b);
    for (int i = parts.Length - 1; i >= 0; i--)
    {
        yield return string.Join("-", parts.Where((_, j) => j != i));
    }
}

string[] MakeSuperSet(string a, string b)
{
    var parts1 = a.Split('-');
    var parts2 = b.Split('-');
    var parts = parts1.Append(parts2.Last()).ToArray();
    return parts;
}

IEnumerable<string[]> ReadConnections(string file)
{
    return File.ReadAllLines(file)
        .Select(l => l.Split('-').OrderBy(v => v).ToArray());
}
