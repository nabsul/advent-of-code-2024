var dirs = new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

IEnumerable<(int x, int y)> input = File.ReadAllLines("input.txt")
    .Select(l => l.Split(","))
    .Select((p) => (x: int.Parse(p[0]), y: int.Parse(p[1])));

var res = MapDistances(1024);
if (res == int.MaxValue)
{
    Console.WriteLine("Part 1: No path found");
    return;
}
Console.WriteLine($"Part 1: {res}");

// start part 2

var (min, max) = (0, input.Count());
int mid = 0;

foreach (int i in Enumerable.Range(2850, 100))
{
    Console.WriteLine($"Part 2: {mid + i} - {MapDistances(mid + i)} - {input.Take(mid + i).Last()}");
}

int MapDistances(int take)
{
    var grid = new int[71, 71];
    for (int y = 0; y <= 70; y++)
    for (int x = 0; x <= 70; x++)
        grid[x, y] = int.MaxValue;
    bool InBounds((int x, int y) p) => p.x >= 0 && p.x <= 70 && p.y >= 0 && p.y <= 70;
    int GridGet((int x, int y) p) => grid[p.x, p.y];
    void GridSet((int x, int y) p, int v) => grid[p.x, p.y] = v;
    Func<(int, int), (int, int)> Add((int x, int y) p) => ((int x, int y) d) => (p.x + d.x, p.y + d.y);
    foreach (var p in input.Take(take)) GridSet(p, -1);

    Queue<(int x, int y)> q = [];
    grid[0, 0] = 0;
    q.Enqueue((0, 0));

    while (grid[70, 70] == int.MaxValue && q.Count > 0)
    {
        var newQ = new Queue<(int x, int y)>();
        while (q.TryDequeue(out var p))
        {
            var score = GridGet(p) + 1;
            var neighbors = dirs.Select(Add(p))
                .Where(n => InBounds(n) && GridGet(n) == int.MaxValue);
            foreach (var n in neighbors)
            {
                GridSet(n, score);
                newQ.Enqueue(n);
            }
        }
        q = newQ;
    }

    return grid[70, 70];
}
