using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Drawing.Processing;

public class Part2
{
    public static int Solve(IEnumerable<Robot> robots, int start, int incr, int end, (int Cols, int Rows) size)
    {
        char[][] map = Enumerable.Range(0, size.Rows)
            .Select(y => Enumerable.Range(0, size.Cols).Select(x => ' ').ToArray())
            .ToArray();
        
        var curr = new HashSet<Point>();
        IncrementPositions(robots, start, size);

        var t = start;
        var bots = robots.ToArray();
        while (t <= end)
        {
            t += incr;
            IncrementPositions(bots, incr, size);
            Console.Clear();
            var pos = bots.Select(b => b.Position);
            PrintMap(map, curr, pos, t, size);
            SaveGifFile(pos, t, size);
        }

        return 0;
    }

    private static void SaveGifFile(IEnumerable<Point> newPositions, int t, (int Cols, int Rows) size)
    {
        using Image image = new Image<Rgba32>(size.Cols, size.Rows);

        image.Mutate(c => c.BackgroundColor(Color.White));
        foreach (var p in newPositions)
        {
            var rect = new RectangleF(p.X, p.Y, 1, 1);
            image.Mutate(c => c.Fill(Color.Black, rect));
        }

        // Save the image as GIF
        image.Save($"img/output_{t:D6}.gif", new GifEncoder());
    }


    private static void PrintMap(char[][] map, HashSet<Point> curr, IEnumerable<Point> newPositions, int t, (int Cols, int Rows) size)
    {
        foreach (var p in curr.ToArray())
        {
            map[p.Y][p.X] = ' ';
            curr.Remove(p);
        }

        foreach (var p in newPositions)
        {
            map[p.Y][p.X] = '#';
            curr.Add(p);
        }

        Console.WriteLine($"Map at time {t}:");
        for (int y = 0; y < size.Rows; y++)
        {
            Console.WriteLine(map[y]);
        }
    }

    private static void IncrementPositions(IEnumerable<Robot> robot, int seconds, (int Cols, int Rows) size)
    {
        foreach (var r in robot)
        {
            r.Position = IncrementPosition(r, seconds, size);
        }
    }

    private static Point IncrementPosition(Robot robot, int seconds, (int Cols, int Rows) size)
    {
        return new Point(
            IncrementInt(robot.Position.X, robot.Velocity.X * seconds, size.Cols),
            IncrementInt(robot.Position.Y, robot.Velocity.Y * seconds, size.Rows)
        );
    }

    private static int IncrementInt(int value, int increment, int max)
    {
        var v = (value + increment) % max;
        return v < 0 ? v + max : v;
    }
}
