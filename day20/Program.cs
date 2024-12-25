var dirs = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

Solve("test.txt", 1, 2, false);
Solve("input.txt", 100, 2, false);
Solve("test.txt", 50, 20, true);
Solve("input.txt", 100, 20, false);

void Solve(string file, int minSaving, int cheatTime, bool showAll = false)
{
    var map = ParseMap(file);
    var end = FindChar(map, 'E');
    var start = FindChar(map, 'S');

    var dist = FillDistances(map, end);
    var cheats = GetCheats(map, dist, cheatTime)
        .Where(s => s.Saved >= minSaving)
        .GroupBy(s => s.Saved)
        .ToDictionary(g => g.Key, g => g.Count());
    Console.WriteLine("Solutions for {0} with cheat time {2}: {1}", file, cheats.Values.Sum(), cheatTime);
    if (showAll)
    {
        foreach (var (key, val) in cheats.Where(c => c.Key >= minSaving).OrderBy(c => c.Key))
        {
            Console.WriteLine(" - {0}: {1}", key, val);
        }
    }
}

int[,] FillDistances(char[,] map, (int, int) start)
{
    var res = new int[map.GetLength(0), map.GetLength(1)];
    foreach (var (i, j) in Scan(map))
    {
        res[i, j] = int.MaxValue;
    }
    res[start.Item1, start.Item2] = 0;
    var q = new Queue<(int, int)>();
    q.Enqueue(start);

    while (q.Count > 0)
    {
        var p0 = q.Dequeue();
        foreach (var d in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
        {
            var p1 = Add(p0, d);
            var val = Get(res, p0) + 1;
            if (!InBounds(map, p1) || Get(map, p1) == '#' || val > Get(res, p1)) continue;
            Set(res, p1, val);
            q.Enqueue(p1);
       }
    }

    return res;
}

IEnumerable<((int, int) Start, (int, int) End, int Saved)> GetCheats(char[,] map, int[,] dist, int time)
{
    foreach (var p in Scan(map))
    {
        var c = Get(map, p);
        if (c == 'E' || c == '#') continue;
        foreach (var cheat in GetCheatSavings(p, map, dist, time))
        {
            yield return cheat;
        }
    }
}

IEnumerable<((int, int) Start, (int, int) End, int Saved)> GetCheatSavings((int, int) start, char[,] map, int[,] dist, int time)
{
    foreach (var p in GetAllSurrounding(start, time))
    {
        if (!InBounds(dist, p)) continue;
        if (Get(map, p) == '#') continue;
        if (Get(dist, p) == int.MaxValue) continue;
        var saved = Get(dist, start) - Get(dist, p) - Dist(start, p);
        if (saved <= 0) continue;
        yield return (start, p, saved);
    }
}

IEnumerable<(int, int)> GetAllSurrounding((int, int) p, int d)
{
    foreach (var i in Enumerable.Range(1, d))
    foreach (var j in Enumerable.Range(0, i))
    {
        yield return Add(p, (i-j, j));
        yield return Add(p, (j-i, -j));
        yield return Add(p, (-j, i-j));
        yield return Add(p, (j, j-i));
    }
}

void FindCheats((int, int) start, (int, int) curr, int[,] dist, Dictionary<int, int> res, HashSet<(int, int)> visited, int time)
{
    if (time < 0) return;
    visited.Add(curr);

    var currDist = dist[curr.Item1, curr.Item2];
    if (currDist != int.MaxValue)
    {
        var startDist = dist[start.Item1, start.Item2];
        var regDist = Math.Abs(curr.Item1 - start.Item1) + Math.Abs(curr.Item2 - start.Item2);
        var timeSaved = startDist - currDist - regDist;

        if (timeSaved > 0)
        {
            res[timeSaved] = res.TryGetValue(timeSaved, out var val) ? val + 1 : 1;
        }
    }

    var neighbors = from d in dirs select Add(curr, d) into n
                    where InBounds(dist, n) && !visited.Contains(n)
                    select n;

    foreach (var n in neighbors)
    {
        FindCheats(start, n, dist, res, visited, time - 1);
    }
}

char[,] ParseMap(string file)
{
    var arr = File.ReadAllLines(file);
    var res = new char[arr.Length, arr[0].Length];
    for (int i = 0; i < arr.Length; i++)
    {
        for (int j = 0; j < arr[0].Length; j++)
        {
            res[i, j] = arr[i][j];
        }
    }
    return res;
}

(int, int) FindChar(char[,] map, char c)
{
    for (int i = 0; i < map.GetLength(0); i++)
    {
        for (int j = 0; j < map.GetLength(1); j++)
        {
            if (map[i, j] == c)
            {
                return (i, j);
            }
        }
    }
    throw new Exception("Char not found");
}

bool InBounds<T>(T[,] arr, (int, int) p)
{
    var (i, j) = p;
    return i >= 0 && i < arr.GetLength(0) && j >= 0 && j < arr.GetLength(1);
}

(int, int) Add((int, int) a, (int, int) b) => (a.Item1 + b.Item1, a.Item2 + b.Item2);
int Dist((int, int) a, (int, int) b) => Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
T Get<T>(T[,] arr, (int, int) p) => arr[p.Item1, p.Item2];
void Set<T>(T[,] arr, (int, int) p, T val) => arr[p.Item1, p.Item2] = val;

IEnumerable<(int, int)> Scan<T>(T[,] arr)
{
    for (int i = 0; i < arr.GetLength(0); i++)
    {
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            yield return (i, j);
        }
    }
}
