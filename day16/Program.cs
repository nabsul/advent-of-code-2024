
Solve("test1.txt");
Solve("test2.txt");
Solve("input.txt");

const long StepScore = 1;
const long TurnScore = 1000;

var dirs = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
(int, int) Add((int, int) a, (int, int) b) => (a.Item1 + b.Item1, a.Item2 + b.Item2);
(int, int)[] LeftRight((int, int) dir) => [(dir.Item2, -dir.Item1), (-dir.Item2, dir.Item1)];
(int, int) Flip((int, int) dir) => (-dir.Item1, -dir.Item2);

void Solve(string file)
{
    var (blocked, start, end) = ReadMap(file);

    var scores = FillScores(start, blocked);
    var bestScore = dirs.Select(d => scores.TryGetValue((end, d), out var score) ? score : int.MaxValue).Min();
    var spots = ListPaths(start, end, bestScore, scores);

    Console.WriteLine($"{file}: {bestScore} - {solver.Spots}");
}

Dictionary<((int, int), (int, int)), int> FillScores((int, int) start, HashSet<(int, int)> blocked)
{
    var scores = new Dictionary<((int, int), (int, int)), int>();
    var queue = new Queue<(int, int)>();
    queue.Enqueue(start);

    while (queue.Count > 0)
    {
        var next = new Queue<(int, int)>();
        var p = queue.Dequeue();
    }

    return scores;
}

(HashSet<(int, int)> blocked, (int, int) start, (int, int) end) ReadMap(string file)
{
    var start = (0, 0);
    var end = (0, 0);
    var blocked = new HashSet<(int, int)>();
    var lines = File.ReadAllLines(file);
    var cells = new char[lines.Length, lines[0].Length];
    foreach (var i in Enumerable.Range(0, lines.Length))
    {
        var line = lines[i];
        foreach (var j in Enumerable.Range(0, line.Length))
        {
            var c = line[j];
            if (c == 'S') start = (i, j);
            else if (c == 'E') end = (i, j);
            else if (c == '#') blocked.Add((i, j));
            cells[i, j] = line[j];
        }
    }

    return (blocked, start, end);
}