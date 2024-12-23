using System.Text;

var debug = true;
const char PRESS = 'A';
const char UP = '^';
const char DOWN = 'v';
const char LEFT = '<';
const char RIGHT = '>';
var keyPad1 = ReadKeyPad("keypad1.txt");
var keyPad2 = ReadKeyPad("keypad2.txt");
Dictionary<char, (int, int)> dirs = new ()
{
    [UP] = (-1, 0),
    [DOWN] = (1, 0),
    [LEFT] = (0, -1),
    [RIGHT] = (0, 1),
};

Solve("test1.txt", 2);
//Console.WriteLine();
//Reverse("v<<A>>^AvA^Av<<A>>^AAv<A<A>>^AAvAA^<A>Av<A>^AA<A>Av<A<A>>^AAAvA^<A>A");
Solve("input.txt", 2);
//Solve("input.txt", 25);

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
    var start = GenerateSequences(line, keyPad1); // num keys
    while (numLayers-- > 0)
    {
        start = start.SelectMany(l => GenerateSequences(l, keyPad2)); // to robot N
    }
    return start;
}

IEnumerable<string> GenerateSequences(string line, Dictionary<char, (int, int)> keyPad)
{
    var validPositions = keyPad.Values.ToHashSet();
    IEnumerable<string> res = [""];
    var pos = keyPad['A'];
    foreach (var c in line)
    {
        var newPos = keyPad[c];
        var vals = MoveTo(pos, newPos, keyPad, validPositions).ToArray();
        res = res.SelectMany(p => vals.Select(v => p + v + PRESS));
        pos = newPos;
    }

    return res;
}

IEnumerable<string> MoveTo((int, int) pos1, (int, int) pos2, Dictionary<char, (int, int)> keys, HashSet<(int, int)> validPositions)
{
    var (i, j) = Diff(pos2, pos1);
    Dictionary<char, int> moves = [];
    if (j > 0) moves[RIGHT] = j;
    if (i > 0) moves[DOWN] = i;
    if (i < 0) moves[UP] = -i;
    if (j < 0) moves[LEFT] = -j;

    var vals = moves.Select(p => string.Join("", Enumerable.Repeat(p.Key, p.Value))).ToArray();

    if (moves.Count == 0) return [""];
    if (moves.Count == 1) return vals;

    if (moves.Count != 2) throw new Exception("Invalid move count");

    vals = [vals[0] + vals[1], vals[1] + vals[0]];
    return vals.Where(v => IsValidPath(v, pos1, validPositions));
}

bool IsValidPath(string path, (int, int) start, HashSet<(int, int)> keys)
{
    var pos = start;
    foreach (var c in path)
    {
        pos = Add(pos, dirs[c]);
        if (!keys.Contains(pos)) return false;
    }
    return true;
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


void Reverse(string line)
{
    Console.WriteLine($"Reversing {line}");
    line = Expand(line, keyPad2);
    Console.WriteLine($"Robot 2: {line}");
    line = Expand(line, keyPad2);
    Console.WriteLine($"Robot 1: {line}");
    line = Expand(line, keyPad1);
    Console.WriteLine($"Result: {line}");

    string Expand(string line, Dictionary<char, (int, int)> keyPad)
    {
        var res = new StringBuilder();
        var pos = keyPad['A'];
        foreach (var c in line)
        {
            if (c != 'A')
            {
                var d = dirs[c];
                pos = Add(pos, d);
                continue;
            }

            var val = keyPad.First(x => x.Value == pos).Key;
            res.Append(val);
        }

        return res.ToString();
    }
}

