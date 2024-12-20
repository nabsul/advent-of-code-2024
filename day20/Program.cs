Solve("test.txt");

void Solve(string file)
{
    var map = ParseMap(file);
    var end = FindChar(map, 'E');
    var start = FindChar(map, 'S');

    var dist = FillDistances(map, end);
    foreach (var i in Enumerable.Range(0, map.GetLength(0)))
    {
        foreach (var j in Enumerable.Range(0, map.GetLength(1)))
        {
            Console.Write(dist[i, j] == int.MaxValue ? "X" : (dist[i, j] % 10).ToString());
        }
        Console.WriteLine();
    }

    var cheats = GetCheats(start, dist);
    Console.WriteLine("Solutions for {0}", file);
    foreach (var (key, val) in cheats.OrderBy(c => c.Key))
    {
        Console.WriteLine(" - {0}: {1}", key, val);
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
    if (!InBounds(map, x, y)) return false;
    if (map[x, y] == '#') return false;
    if (dist[x, y] <= val) return false;
    dist[x, y] = val;
    return true;
}

Dictionary<int, int> GetCheats((int, int) start, int[,] dist)
{
    var res = new Dictionary<int, int>() {};
    var q = new Queue<(int, int)>();
    q.Enqueue(start);
    while (q.Count > 0)
    {
        var (x, y) = q.Dequeue();
        CheckCheats(res, dist, x, y);
    }
    return res;
}

void CheckCheats(Dictionary<int, int> res, int[,] dist, int x, int y)
{
    foreach (var (dx, dy) in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
    {
        var (wx, wy) = (x + dx, y + dy);
        if (!InBounds(dist, wx, wy) || dist[wx, wy] != int.MaxValue) continue;
        var (px, py) = (wx + dx, wy + dy);
        if (!InBounds(dist, px, py) || dist[px,py] == int.MaxValue) continue;
        var saved = dist[px, py] - dist[x, y] - 2;
        if (saved <= 0) continue;
        if (!res.TryGetValue(saved, out var cur)) cur = 0;
        res[saved] = cur + 1;
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

bool InBounds<T>(T[,] arr, int i, int j)
{
    return i >= 0 && i < arr.GetLength(0) && j >= 0 && j < arr.GetLength(1);
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
