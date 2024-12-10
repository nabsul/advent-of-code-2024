using System.Net;

namespace Day10;

public class Part1(int[][] map)
{
    public int Solve()
    {
        return GetPoints(map)
            .Where(p => map[p.r][p.c] == 0)
            .Select(p => CountPaths(map, p))
            .Sum();
    }

    public virtual int CountPaths(int[][] map, (int r, int c) p)
    {
        return GetPeaks(map, p).Distinct().Count();
    }

    public static readonly (int r, int c)[] offsets =
    [
        (-1, 0),
        (+1, 0),
        (0, -1),
        (0, +1),
    ];


    public static IEnumerable<(int r, int c)> GetPeaks(int[][] map, (int r, int c) p)
    {
        if (map[p.r][p.c] == 9)
        {
            return Enumerable.Repeat(p, 1);
        }

        return offsets
            .Select(o => (r: p.r + o.r, c: p.c + o.c))
            .Where(np => InBounds(map, np))
            .Where(np => map[np.r][np.c] == map[p.r][p.c] + 1)
            .SelectMany(d => GetPeaks(map, d));        
    }

    public static bool InBounds(int[][] map, (int r, int c) p)
    {
        return p.r >= 0 && p.r < map.Length && p.c >= 0 && p.c < map[p.r].Length;
    }

    public static IEnumerable<(int r, int c)> GetPoints(int[][] map)
    {
        for (var r = 0; r < map.Length; r++)
        {
            for (var c = 0; c < map[r].Length; c++)
            {
                yield return (r, c);
            }
        }
    }
}
 