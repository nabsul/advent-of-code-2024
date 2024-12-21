const int StepScore = 1;
const int TurnScore = 1000;

var dirs = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
(int, int) Add((int, int) a, (int, int) b) => (a.Item1 + b.Item1, a.Item2 + b.Item2);
(int, int) Rotate1((int, int) dir) => (dir.Item2, -dir.Item1);
(int, int) Rotate2((int, int) dir) => (-dir.Item2, dir.Item1);
(int, int) Flip((int, int) dir) => (-dir.Item1, -dir.Item2);

Solve("test1.txt");
Solve("test2.txt");
Solve("input.txt");

void Solve(string file)
{
    var (blocked, start, end) = ReadMap(file);

    var scores = FillScores(start, blocked);
    var bestScore = dirs.Select(d => scores.TryGetValue((end, d), out var score) ? score : int.MaxValue).Min();
    var spots = CountPathBlocks(start, end, bestScore, scores);

    Console.WriteLine($"{file}: {bestScore} - {spots}");
}

int CountPathBlocks((int, int) start, (int, int) end, int bestScore, Dictionary<((int, int), (int, int)), int> scores)
{
    return ListAllPathBlocks(start, end, bestScore, scores).Distinct().Count();
}

IEnumerable<(int, int)> ListAllPathBlocks((int, int) start, (int, int) end, int bestScore, Dictionary<((int, int), (int, int)), int> scores)
{
    foreach (var d in dirs)
    {
        if (!scores.TryGetValue((end, d), out var score) || score != bestScore) continue;
        foreach (var res in ListSinglePathBlocks(end, d, scores))
        {
            yield return res;
        }
    }
}

IEnumerable<(int, int)> ListSinglePathBlocks((int, int) p, (int, int) d, Dictionary<((int, int), (int, int)), int> scores)
{
    if (p == (0, 0)) return [];

    var score = scores[(p, d)];
    return TryNeighbor(p, d, score + StepScore, scores)
        .Concat(TryNeighbor(p, Rotate1(d), score + StepScore + TurnScore, scores))
        .Concat(TryNeighbor(p, Rotate2(d), score + StepScore + TurnScore, scores))
        .Append(p);
}

IEnumerable<(int, int)> TryNeighbor((int, int) p, (int, int) d, int score, Dictionary<((int, int), (int, int)), int> scores)
{
    var pos = Add(p, Flip(d));
    if (!scores.TryGetValue((pos, d), out var nextScore) || nextScore != score) yield break;
    yield return pos;
}

Dictionary<((int, int), (int, int)), int> FillScores((int, int) start, HashSet<(int, int)> blocked)
{
    var scores = new Dictionary<((int, int), (int, int)), int>();
    scores[(start, (0, 1))] = 0;
    var queue = new Queue<(int, int)>();
    queue.Enqueue(start);

    while (queue.Count > 0)
    {
        var next = new Queue<(int, int)>();
        while (queue.TryDequeue(out var p))
        {
            foreach (var n in FillPoint(p, blocked, scores))
            {
                next.Enqueue(n);
            }
        }
        queue = next;
    }

    return scores;
}

IEnumerable<(int, int)> FillPoint((int, int) p, HashSet<(int, int)> blocked, Dictionary<((int, int), (int, int)), int> scores)
{
    if (blocked.Contains(p)) yield break;

    // Check that rotations are minimized
    foreach (var d in dirs)
    {
        if (!scores.TryGetValue((p, d), out var score)) continue;
        SetIfSmaller(p, Rotate1(d), score + TurnScore, scores);
        SetIfSmaller(p, Rotate2(d), score + TurnScore, scores);
        SetIfSmaller(p, Flip(d), score + 2*TurnScore, scores);
    }

    // Check next steps are minimized
    foreach (var d in dirs)
    {
        var score = scores[(p, d)];
        var next = Add(p, d);
        if (blocked.Contains(next)) continue;
        if (SetIfSmaller(next, d, score + StepScore, scores)) yield return next;
    }
}

bool SetIfSmaller((int, int) p, (int, int) d, int score, Dictionary<((int, int), (int, int)), int> scores)
{
    if (scores.TryGetValue((p, d), out var current) && current <= score) return false;
    scores[(p, d)] = score;
    return true;
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