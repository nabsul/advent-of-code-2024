using System.Text;

var debug = true;
var keyPad1 = ReadKeyPad("keypad1.txt");
var keyPad2 = ReadKeyPad("keypad2.txt");
Dictionary<char, (int, int)> dirs = new ()
{
    ['^'] = (-1, 0),
    ['v'] = (1, 0),
    ['<'] = (0, -1),
    ['>'] = (0, 1),
};

//Solve("test1.txt");
//Console.WriteLine();
Reverse("<v<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A");
Reverse("v<<A>>^AvA^Av<<A>>^AAv<A<A>>^AAvAA^<A>Av<A>^AA<A>Av<A<A>>^AAAvA^<A>A");
//Solve("input.txt");

void Solve(string file)
{
    Console.WriteLine($"Solving {file}");
    var totalScore = 0;
    foreach (var line in File.ReadAllLines(file))
    {
        var sequence = string.Join("", GenerateKeys(line));
        var val = int.Parse(line[..^1]);
        var score = sequence.Length * val;
        totalScore += score;
        Console.WriteLine($"{line} -> {sequence} -> {sequence.Length}x{val}={score}");
    }
    Console.WriteLine($"Total score: {totalScore}");
}

IEnumerable<char> GenerateKeys(string line)
{
    if (debug) Console.WriteLine($"Generating keys for {line}");
    line = GenerateSequence(line, keyPad1); // to robot 1
    if (debug) Console.WriteLine($"Robot 1: {line}");
    line = GenerateSequence(line, keyPad2); // to robot 2
    if (debug) Console.WriteLine($"Robot 2: {line}");
    line = GenerateSequence(line, keyPad2); // to human
    if (debug) Console.WriteLine($"Human: {line}");
    return line;
}

string GenerateSequence(string line, Dictionary<char, (int, int)> keyPad)
{
    var res = Enumerable.Empty<char>();
    var pos = keyPad['A'];
    foreach (var c in line)
    {
        var newPos = keyPad[c];
        res = res.Concat(PressButton(pos, newPos, c));
        pos = newPos;
    }

    return string.Join("", res);
}

IEnumerable<char> PressButton((int, int) pos1, (int, int) pos2, char c)
{
    var res = Enumerable.Empty<char>();
    var (i, j) = Diff(pos2, pos1);
    if (j > 0) res = res.Concat(Enumerable.Repeat('>', j));
    if (i > 0) res = res.Concat(Enumerable.Repeat('v', i));
    if (i < 0) res = res.Concat(Enumerable.Repeat('^', -i));
    if (j < 0) res = res.Concat(Enumerable.Repeat('<', -j));
    res = res.Append('A');

    return res;
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
}

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
