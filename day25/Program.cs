var debug = true;

Solve("test.txt");
Solve("input.txt");


void Solve(string file)
{
    var (locks, keys) = ReadInput(file);
    if (debug) Console.WriteLine($"Locks:\n{string.Join("\n", locks.Select(l => string.Join(" ", l)))}");
    if (debug) Console.WriteLine($"Keys:\n{string.Join("\n", keys.Select(l => string.Join(" ", l)))}");
    var count = locks.SelectMany(l => keys.Select(k => (l, k))).Count(x => CanFit(x.l, x.k));
    Console.WriteLine($"File {file} has {count} combos");
}

bool CanFit(int[] l, int[] k) => l.Zip(k, (l, k) => l + k).All(x => x <= 5);

(int[][] locks, int[][] keys) ReadInput(string file)
{
    var locks = new List<int[]>();
    var keys = new List<int[]>();

    foreach (var map in ReadMaps(file))
    {
        if (debug) Console.WriteLine($"Map:\n{string.Join("\n", map)}");
        if (map[0][0] == '#')
        {
            locks.Add(GetHeights(map));
        }
        else
        {
            keys.Add(GetHeights(map.AsEnumerable().Reverse()));
        }
    }
    return (locks.ToArray(), keys.ToArray());
}

IEnumerable<List<string>> ReadMaps(string file)
{
    var buff = File.ReadAllText(file);
    var maps = buff.Split("\n\n");
    return maps.Select(m => m.Split("\n").ToList());
}

int[] GetHeights(IEnumerable<string> buff)
{
    var it = buff.GetEnumerator();
    it.MoveNext();
    var res = Enumerable.Repeat(0, it.Current.Length).ToArray();
    while (it.MoveNext())    
    {
        for (int i = 0; i < it.Current.Length; i++)
        {
            if (it.Current[i] == '#')
            {
                res[i]++;
            }
        }
    }
    if (debug) Console.WriteLine($"Heights: {string.Join(" ", res)}");
    return res;
}
