var dirs = new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

IEnumerable<(int x, int y)> input = File.ReadAllLines("input.txt")
    .Select(l => l.Split(","))
    .Select((p) => (x: int.Parse(p[0]), y: int.Parse(p[1])));

var grid = new int[71, 71];
for (int y = 0; y <= 70; y++)
for (int x = 0; x <= 70; x++)
    grid[x, y] = int.MaxValue;
bool InBounds((int x, int y) p) => p.x >= 0 && p.x <= 70 && p.y >= 0 && p.y <= 70;
int GridGet((int x, int y) p) => grid[p.x, p.y];
void GridSet((int x, int y) p, int v) => grid[p.x, p.y] = v;
Func<(int, int), (int, int)> Add((int x, int y) p) => ((int x, int y) d) => (p.x + d.x, p.y + d.y);
foreach (var p in input.Take(1024)) GridSet(p, -1);

MapDistances();
if (grid[70, 70] == int.MaxValue)
{
    Console.WriteLine("Part 1: No path found");
    return;
}
Console.WriteLine($"Part 1: {grid[70, 70]}");

// start part 2

grid = new int[71, 71];
for (int y = 0; y <= 70; y++)
for (int x = 0; x <= 70; x++)
    grid[x, y] = int.MaxValue;
MapDistances();

foreach (var p in input)
{
    GridSet(p, -1);
    UpdateCells(p);

    if (grid[70, 70] == int.MaxValue)
    {
        Console.WriteLine($"Part 2: {p}");
        Print();
        return;
    }
}

Print();
Console.WriteLine("Part 2: No blocking found");

void UpdateCells((int x, int y) p)
{
    var visited = new HashSet<(int x, int y)>();
    foreach (var x in dirs.Select(Add(p)))
    {
        UpdateCell(x, visited);
    }
}

void UpdateCell((int x, int y) p, HashSet<(int x, int y)> visited)
{
    var skip = !InBounds(p) || visited.Contains(p) || GridGet(p) == int.MaxValue || GridGet(p) == -1;
    if (skip) return;
    visited.Add(p);

    var v = GridGet(p);
    if (dirs.Select(Add(p)).Where(InBounds).Any(n => GridGet(n) == v - 1))
    {
        foreach (var x in dirs.Select(Add(p)).Where(InBounds).Where(n => GridGet(n) == int.MaxValue))
        {
            GridSet(x, v + 1);
        }
        return;
    }

    GridSet(p, int.MaxValue);
    foreach (var x in dirs.Select(Add(p)))
    {
        UpdateCell(x, visited);
    }
}

void MapDistances()
{
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
}

void Print()
{
    for (int y = 0; y <= 70; y++)
    {
        for (int x = 0; x <= 70; x++)
        {
            var v = grid[x, y];
            if (v == int.MaxValue) Console.Write("XX ");
            else if (v == -1) Console.Write("[] ");
            else Console.Write($"{v % 100:D2} ");
        }
        Console.WriteLine();
    }
}
