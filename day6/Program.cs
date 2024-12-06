var directions = new Dictionary<char, (int Row, int Col)>
{
    { '^', (-1, 0) },
    { '>', (0, 1) },
    { 'v', (1, 0) },
    { '<', (0, -1) }
};

var map = ReadData();
var startPos = FindGuardPosition(map);
var startDir = map[startPos.Row][startPos.Col];
map[startPos.Row][startPos.Col] = '.';

var xCount = CountPathLength(map, startPos, startDir);
Console.WriteLine($"X count: {xCount}");

var loopCount = 0;
var rows = map.Length;
var cols = map[0].Length;
for (int i = 0; i < rows; i++)
{
    for (int j = 0; j < cols; j++)
    {
        if ((i, j) == startPos || map[i][j] != '.') continue;
        map[i][j] = '#';
        if (-1 == CountPathLength(map, startPos, startDir))
        {
            loopCount++;
        }
        map[i][j] = '.';
    }
}

Console.WriteLine($"Loop count: {loopCount}");

int CountPathLength(char[][] map, (int Row, int Col) pos, char dir)
{
    var visited = new Dictionary<(int, int), char>();

    while (IsInMap(map, pos))
    {
        if (visited.TryGetValue(pos, out var prevDir))
        {
            if (prevDir == dir) return -1; // loop
        }
        else
        {
            visited.Add(pos, dir);
        }

        while (true)
        {
            var (r, c) = directions[dir];
            var newPos = (Row: pos.Row + r, Col: pos.Col + c);
            if (!IsInMap(map, newPos) || map[newPos.Row][newPos.Col] != '#')
            {
                pos = newPos;
                break;
            }

            dir = RotateRight(dir);
        }
    }

    return visited.Count;
}

char RotateRight(char dir)
{
    switch (dir)
    {
        case '^': return '>';
        case '>': return 'v';
        case 'v': return '<';
        case '<': return '^';
        default: throw new Exception("Invalid direction");
    }
}

bool IsInMap(char[][] map, (int Row, int Col) pos)
{
    return pos.Row >= 0 && pos.Row < map.Length && pos.Col >= 0 && pos.Col < map[0].Length;
}

(int Row, int Col) FindGuardPosition(char[][] map)
{
    for (int i = 0; i < map.Length; i++)
    {
        for (int j = 0; j < map[0].Length; j++)
        {
            if (directions.ContainsKey(map[i][j]))
            {
                return (i, j);
            }
        }
    }

    return (-1, -1);
}

char[][] ReadData()
{
    return File.ReadAllLines("input.txt")
        .Select(l => l.ToCharArray())
        .ToArray();
}

