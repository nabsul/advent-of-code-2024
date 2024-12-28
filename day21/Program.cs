const char PRESS = 'A';
const char UP = '^';
const char DOWN = 'v';
const char LEFT = '<';
const char RIGHT = '>';
var numPad = ReadKeyPad("keypad1.txt");
var numPadPositions = numPad.Values.ToHashSet();
var arrowPad = ReadKeyPad("keypad2.txt");
var arrowPadPositions = arrowPad.Values.ToHashSet();
Dictionary<char, (int, int)> dirs = new ()
{
    [UP] = (-1, 0),
    [DOWN] = (1, 0),
    [LEFT] = (0, -1),
    [RIGHT] = (0, 1),
};
var cache = new Dictionary<string, string[]>();

Solve("test1.txt", 2);
Solve("input.txt", 2);
Solve("input.txt", 25);

void Solve(string file, int numLayers)
{
    Console.WriteLine($"Solving {file}");
    var totalScore = 0;
    foreach (var line in File.ReadAllLines(file))
    {
        var sequence = GetShortestKeys(line, numLayers);
        var val = int.Parse(line[..^1]);
        var score = sequence.Length * val;
        totalScore += score;
        Console.WriteLine($"{line} -> {sequence} -> {sequence.Length}x{val}={score}");
    }
    Console.WriteLine($"Total score: {totalScore}");
}

string GetShortestKeys(string line, int numLayers)
{
    return GenerateKeys(line, numLayers).OrderBy(x => x.Length).First();
}

IEnumerable<string> GenerateKeys(string line, int numLayers)
{
    return GenerateArrowMoves(GenerateNumPadMoves(line), numLayers);
}

IEnumerable<string> GenerateArrowMoves(IEnumerable<string> lines, int numLayers)
{
    var chunks = lines.SelectMany(SplitArrowMoves).Distinct().ToArray();
    var lookup = GenerateChunkLookup(chunks, numLayers);

    foreach (var line in lines)
    {
        IEnumerable<string> res = [""];
        foreach (var chunk in SplitArrowMoves(line))
        {
            var vals = lookup[chunk];
            res = res.SelectMany(p => vals.Select(v => p + v)).ToArray();
        }
        foreach (var l in res) yield return l;
    }
}

Dictionary<string, string[]> GenerateChunkLookup(string[] chunks, int numLayers)
{
    Console.WriteLine($"Generating lookup for {numLayers} layers");
    var res = new Dictionary<string, string[]>();
    if (numLayers == 1)
    {
        foreach (var chunk in chunks)
        {
            res.Add(chunk, SolveSingleChunk(chunk));
        }
        return res;
    }

    var prev = GenerateChunkLookup(chunks, numLayers - 1);
    chunks = prev.Values.SelectMany(v => v.SelectMany(SplitArrowMoves)).Distinct().ToArray();
    var lookup = GenerateChunkLookup(chunks, 1);

    foreach (var (key, values) in prev)
    {
        IEnumerable<string> final = [];
        foreach (var val in values)
        {
            IEnumerable<string> part = [""];
            foreach (var chunk in SplitArrowMoves(val))
            {
                var vals = lookup[chunk];
                part = part.SelectMany(p => vals.Select(v => p + v)).ToArray();
            }
            final = final.Concat(part);
        }
        res.Add(key, final.ToArray());
    }

    return Prune(res);
}

Dictionary<string, string[]> Prune(Dictionary<string, string[]> lookup)
{
    var res = new Dictionary<string, string[]>();
    foreach (var (key, values) in lookup)
    {
        var min = values.Min(v => v.Length);
        res.Add(key, values.Where(v => v.Length == min).ToArray());
    }
    return res;
}

IEnumerable<string> SplitArrowMoves(string line)
{
    var start = 0;
    for (int i = 0; i < line.Length; i++)
    {
        if (line[i] == PRESS)
        {
            yield return line[start..(i+1)];
            start = i + 1;
        }
    }
}

string[] SolveSingleChunk(string chunk)
{
    IEnumerable<string> res = [""];
    var pos = arrowPad[PRESS];
    foreach (var c in chunk)
    {
        var newPos = arrowPad[c];
        var vals = MoveTo(arrowPadPositions, pos, newPos).ToArray();
        res = res.SelectMany(p => vals.Select(v => p + v + PRESS));
        pos = newPos;
    }
    return res.ToArray();
}

IEnumerable<string> GenerateNumPadMoves(string line) => GenerateNumPadMovesInner(line, numPad['A']);
IEnumerable<string> GenerateNumPadMovesInner(string line, (int, int) pos, int idx = 0)
{
    if (idx == line.Length) return [""];

    var c = line[idx];
    var newPos = numPad[c];
    return MoveTo(numPadPositions, pos, newPos)
        .SelectMany(move => GenerateNumPadMovesInner(line, newPos, idx + 1)
        .Select(next => move + PRESS + next));
}

IEnumerable<string> MoveTo(HashSet<(int, int)> validPositions, (int, int) pos1, (int, int) pos2)
{
    var (i, j) = Diff(pos2, pos1);
    var moves = Enumerable.Repeat(RIGHT, j > 0 ? j : 0)
        .Concat(Enumerable.Repeat(DOWN, i > 0 ? i : 0))
        .Concat(Enumerable.Repeat(UP, i < 0 ? -i : 0))
        .Concat(Enumerable.Repeat(LEFT, j < 0 ? -j : 0))
        .ToArray();

    var vals = new[] { string.Join("", moves), string.Join("", moves.Reverse()) };
    if (vals[0] == vals[1]) return [vals[0]];
    return vals.Where(v => IsValidPath(v));

    bool IsValidPath(string path)
    {
        var pos = pos1;
        foreach (var c in path)
        {
            pos = Add(pos, dirs[c]);
            if (!validPositions.Contains(pos)) return false;
        }
        return true;
    }
}


Dictionary<char, (int, int)> ReadKeyPad(string file)
{
    var res = new Dictionary<char, (int, int)>();
    var lines = File.ReadAllLines(file);
    for (int i = 0; i < lines.Length; i++)
    {
        for (int j = 0; j < lines[i].Length; j++)
        {
            if (lines[i][j] != ' ')
            {
                res.Add(lines[i][j], (i, j));
            }
        }
    }
    return res;
}

(int, int) Diff((int, int) a, (int, int) b) => (a.Item1 - b.Item1, a.Item2 - b.Item2);
(int, int) Add((int, int) a, (int, int) b) => (a.Item1 + b.Item1, a.Item2 + b.Item2);
