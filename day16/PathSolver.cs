namespace Day16;

using Point = (int R, int C);

public class PathSolver(Map map)
{
    const long StepScore = 1;
    const long TurnScore = 1000;

    long BestScore = long.MaxValue;
    long Spots = 0;

    public static void Solve(string file)
    {
        var map = new Map($"inputs/{file}");
        var solver = new PathSolver(map);
        solver.Solve();
        //solver.CountSpots();
        Console.WriteLine($"{file}: {solver.BestScore} - {solver.Spots}");
    }

    HashSet<Point> spots = [];
    HashSet<Point> visited = [];

    void CountSpots()
    {
        CountSpots(start, (0, 1), 0);
        Spots = spots.Count;
    }

    bool CountSpots(Point p, Point dir, long score)
    {
        if (score > BestScore || visited.Contains(p)) return false;

        if (p == end)
        {
            spots.Add(p);
            return true;
        }

        var res = false;
        res = res || CountSpots(Add(p, dir), dir, score + StepScore);
        dir = Rotate(dir);
        res = res || CountSpots(Add(p, dir), dir, score + TurnScore + StepScore);
        dir = Flip(dir);
        res = res || CountSpots(Add(p, dir), dir, score + TurnScore + StepScore);

        if (res) spots.Add(p);
        return res;        
    }

    void Solve()
    {
        BestScore = Math.Min(Start((0, 1)) - TurnScore, Start((1, 0)));
    }

    Point start = (0, 0);
    Point end = (0, 0);

    long Start(Point dir)
    {
        Dictionary<Point, long> scores = [];
        HashSet<Point> blocked = [];

        foreach (var p in Scan())
        {
            if (map.Get(p) == 'S') start = p;
            else if (map.Get(p) == 'E') end = p;
            else if (map.Get(p) == '#') blocked.Add(p);
        }

        if (start == (0, 0) || end == (0, 0)) throw new Exception("Invalid map");

        scores[start] = 0;
        long result;
        while (!scores.TryGetValue(end, out result))
        {
            if (scores.Count == 0) return long.MaxValue;
            scores = Iterate(scores, blocked, dir);
            dir = Rotate(dir);
        }

        return result;
    }

    static Dictionary<Point, long> Iterate(Dictionary<Point, long> scores, HashSet<Point> blocked, Point dir)
    {
        Dictionary<Point, long> newScores = [];
        foreach (var (p, score) in scores)
        {
            Populate(newScores, blocked, p, dir, score);
            Populate(newScores, blocked, p, Flip(dir), score);
        }

        return newScores;
    }

    static void Populate(Dictionary<Point, long> newScores, HashSet<Point> blocked, Point p, Point dir, long score)
    {
        score += TurnScore;
        while (!blocked.Contains(p = Add(p, dir)))
        {
            if (blocked.Contains(p)) break;
            blocked.Add(p);
            score += StepScore;

            if (!newScores.TryGetValue(p, out var prevScore))
            {
                prevScore = long.MaxValue;
            }

            newScores[p] = Math.Min(prevScore, score);
        }
    }

    IEnumerable<Point> Scan()
    {
        foreach (var i in Enumerable.Range(0, map.Rows))
        {
            foreach (var j in Enumerable.Range(0, map.Columns))
            {
                yield return (i, j);
            }
        }
    }

    private static Point Add(Point a, Point b) => (a.R + b.R, a.C + b.C);
    private static Point Flip(Point p) => (-p.R, -p.C);
    private static Point Rotate(Point p) => (p.C, p.R);
}
