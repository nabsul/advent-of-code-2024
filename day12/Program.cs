(int, int)[] DIRECTIONS = [(0, 1), (1, 0), (0, -1), (-1, 0)];

var map = File.ReadAllLines("input.txt")
    .Select(l => l.ToCharArray())
    .ToArray();

var visited = new HashSet<(int, int)>();

var (cost1, cost2) = GetCosts();
Console.WriteLine($"Part 1: {cost1}");
Console.WriteLine($"Part 2: {cost2}");

(long cost1, long cost2) GetCosts()
{
    long cost1 = 0;
    long cost2 = 0;

    foreach (var pos in ScanMap())
    {
        if (visited.Contains(pos))
        {
            continue;
        }
        var (area, peri, sides) = CalcAreaCost(pos);

        cost1 += area * peri;
        cost2 += area * sides;
    }

    return (cost1, cost2);
}

(long area, long peri, long sides) CalcAreaCost((int, int) pos)
{
    long area = 0;
    long peri = 0;
    long sides = 0;

    var q = new Queue<(int, int)>();
    q.Enqueue(pos);
    visited.Add(pos);

    while (q.TryDequeue(out pos))
    {
        area++;
        var (x, y) = pos;

        foreach (var i in Enumerable.Range(0, 4))
        {
            var d = DIRECTIONS[i];
            var n = (x + d.Item1, y + d.Item2);
            if (!InBounds(n) || map[n.Item1][n.Item2] != map[x][y])
            {
                peri++;

                // only the or left-most part of a side
                var rot = DIRECTIONS[(i + 1) % 4];
                var p2 = (x + rot.Item1, y + rot.Item2);

                var hasNeighbour = InBounds(p2) && map[p2.Item1][p2.Item2] == map[x][y];

                var n2 = (p2.Item1 + d.Item1, p2.Item2 + d.Item2);
                var isOut = !InBounds(n2) || map[n2.Item1][n2.Item2] != map[x][y];

                if (hasNeighbour && isOut)
                {
                    continue;
                }

                sides++;
            }
            else if (!visited.Contains(n))
            {
                q.Enqueue(n);
                visited.Add(n);
            }
        }
    }

    return (area, peri, sides);
}

IEnumerable<(int, int)> ScanMap()
{
    for (int i = 0; i < map.Length; i++)
    {
        for (int j = 0; j < map[i].Length; j++)
        {
            yield return (i, j);
        }
    }
}

bool InBounds((int i, int j) pos) => pos.i >= 0 && pos.i < map.Length && pos.j >= 0 && pos.j < map[pos.i].Length;
