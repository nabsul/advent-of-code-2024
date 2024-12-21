var dirs = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

Solve("test.txt", 1, 2, true);
Solve("input.txt", 100, 2, false);
Solve("test.txt", 50, 50, true);
Solve("input.txt", 100, 50);

void Solve(string file, int minSaving, int cheatTime, bool showAll = false)
{
    var map = ParseMap(file);
    var end = FindChar(map, 'E');
    var start = FindChar(map, 'S');

    var dist = FillDistances(map, end);
    var cheats = GetCheats(start, dist, cheatTime);
    var cheatCount = cheats.Where(c => c.Key >= minSaving).Select(c => c.Value).Sum();
    Console.WriteLine("Solutions for {0} with cheat time {2}: {1}", file, cheatCount, cheatTime);
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
        var (x, y) = q.Dequeue();
        foreach (var (dx, dy) in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
        {
            if (TrySetDistance(map, res, x + dx, y + dy, res[x, y] + 1))
            {
                q.Enqueue((x + dx, y + dy));
            }
        }
    }

    return res;
}

bool TrySetDistance(char[,] map, int[,] dist, int x, int y, int val)
{
    if (!InBounds(map, (x, y))) return false;
    if (map[x, y] == '#') return false;
    if (dist[x, y] <= val) return false;
    dist[x, y] = val;
    return true;
}

Dictionary<int, int> GetCheats((int, int) start, int[,] dist, int time)
{
    var res = new Dictionary<int, int>() {};
    (int, int)[] positions = [start];

    while (positions.Length > 0)
    {
        var next = Enumerable.Empty<(int, int)>();

        foreach (var p in positions)
        {
            var currVal = dist[p.Item1, p.Item2];
            FindCheats(p, p, dist, res, [], time);
            var q = from d in dirs select Add(p, d) into n
                    where InBounds(dist, n) && dist[n.Item1, n.Item2] == currVal - 1
                    select n;
            next = next.Concat(q);
        }

        positions = [.. next];
    }

    return res;
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

(int, int) Add((int, int) a, (int, int) b)
{
    return (a.Item1 + b.Item1, a.Item2 + b.Item2);
}

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
