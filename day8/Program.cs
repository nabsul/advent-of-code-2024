var Rows = 0;
var Cols = 0;
var antennae = new Dictionary<char, List<Position>>();
ReadAntennae();

Console.WriteLine($"Rows: {Rows}, Cols: {Cols}");
Console.WriteLine($"Antennae: {antennae.Sum(a => a.Value.Count)}");

//GenerateMap();

var allAntinodes = antennae.Values
    .SelectMany(p => GetAllPairs(p.Count).SelectMany(pair => GetAntinodes(p[pair.Item1], p[pair.Item2])))
    .Where(IsInBounds)
    .Distinct();

Console.WriteLine($"Part1: {allAntinodes.Count()}");

var allAntinodesPart2 = antennae.Values
    .SelectMany(p => GetAllPairs(p.Count).SelectMany(pair => GetAntinodesPart2(p[pair.Item1], p[pair.Item2])))
    .Distinct();

Console.WriteLine($"Part2: {allAntinodesPart2.Count()}");

void ReadAntennae()
{
    foreach (var line in File.ReadLines("data.txt"))
    {
        Cols = 0;
        foreach (var c in line)
        {
            if (c != '.')
            {
                if (!antennae.TryGetValue(c, out var a))
                {
                    antennae[c] = a = [];
                }
                a.Add(new Position(Rows, Cols));
            }

            Cols++;
        }
        Rows++;
    }
}

IEnumerable<(int, int)> GetAllPairs(int len)
{
    for (var i = 0; i < len; i++)
    {
        for (var j = i + 1; j < len; j++)
        {
            yield return (i, j);
        }
    }
}

IEnumerable<Position> GetAntinodes(Position a, Position b)
{
    var dr = a.R - b.R;
    var dc = a.C - b.C;
    yield return new Position(a.R + dr, a.C + dc);
    yield return new Position(b.R - dr, b.C - dc);
}

IEnumerable<Position> GetAntinodesPart2(Position a, Position b)
{
    var dr = a.R - b.R;
    var dc = a.C - b.C;

    var p = new Position(a.R, a.C);
    while (IsInBounds(p))
    {
        yield return p;
        p = new Position(p.R + dr, p.C + dc);
    }

    p = new Position(b.R, b.C);
    while (IsInBounds(p))
    {
        yield return p;
        p = new Position(p.R - dr, p.C - dc);
    }
}

bool IsInBounds(Position p) => p.R >= 0 && p.R < Rows && p.C >= 0 && p.C < Cols;

void GenerateMap()
{
    var map = new char[Rows, Cols];
    for (var r = 0; r < Rows; r++)
    {
        for (var c = 0; c < Cols; c++)
        {
            map[r, c] = '.';
        }
    }

    foreach (var (c, positions) in antennae)
    {
        foreach (var p in positions)
        {
            map[p.R, p.C] = c;
        }
    }

    Console.WriteLine("Map:");
    for (var r = 0; r < Rows; r++)
    {
        for (var c = 0; c < Cols; c++)
        {
            Console.Write(map[r, c]);
        }
        Console.WriteLine();
    }
}


record Position(int R, int C);

