public class Part1
{
        public static int Solve(IEnumerable<Robot> robots, int seconds, (int Cols, int Rows) size)
    {
        var newPositions = robots.Select(r => IncrementPosition(r, seconds, size)).ToArray();
        var quads = newPositions .Select(p => GetQuadrant(p, size));

        var res = 1;
        foreach (var quad in quads.Where(q => q != 0).GroupBy(q => q))
        {
            res *= quad.Count();
        }

        return res;
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

    private static int GetQuadrant(Point point, (int Cols, int Rows) size)
    {
        var midCol = size.Cols / 2;
        var midRow = size.Rows / 2;

        if (point.X == midCol || point.Y == midRow)
        {
            return 0;
        }

        bool isAbove = point.Y < midRow;
        bool isLeft = point.X < midCol;

        return (isAbove, isLeft) switch
        {
            (true, true) => 1,
            (true, false) => 2,
            (false, true) => 3,
            (false, false) => 4,
        };
    }

}