using System.Security.Cryptography;

public class Solver(IEnumerable<ClawMachine> machines)
{
    public long Solve()
    {
        return machines.Select(Solve).Where(c => c > 0).Sum();
    }

    private long Solve(ClawMachine machine)
    {
        var results = SolveWithMath(machine).ToArray();
        if (results.Length == 0)
        {
            return -1;
        }

        return results.Min(x => x.Cost);
    }

    private IEnumerable<Solution> SolveBruteForce(ClawMachine machine)
    {
        for (long numA = 0; !PastTarget(numA, machine); numA++)
        {
            var start = numA * machine.ButtonA;
            var targ = machine.Prize - start;
            var numB = targ.X / machine.ButtonB.X;
            if (start + numB * machine.ButtonB == machine.Prize)
            {
                yield return new Solution(numA, numB);
            }
        }
    }

    private bool PastTarget(long mult, ClawMachine machine)
    {
        var button = machine.ButtonA;
        var targ = machine.Prize;
        return mult * button.X > targ.X || mult * button.Y > targ.Y;
    }

    // solve using math instead of brute force
    private IEnumerable<Solution> SolveWithMath(ClawMachine m)
    {
        if (AreParallel(m.ButtonA, m.Prize))
        {
            if (AreParallel(m.ButtonB, m.Prize))
            {
                return GetInLine(m.ButtonA, m.ButtonB, m.Prize);
            }
            else
            {
                return SolveSingle(m.ButtonA, m.Prize).Select(a => new Solution(a, 0));
            }
        }
        else
        {
            if (AreParallel(m.ButtonB, m.Prize))
            {
                return SolveSingle(m.ButtonB, m.Prize).Select(b => new Solution(0, b));
            }
            else
            {
                return SolveTwoDimensional(m).Select(x => new Solution(x.Item1, x.Item2));
            }
        }
    }

    /**
    * Solve the equation:
    * ax1 + bx2 = x3 (we can ignore the y because it will be the same ratio)
    * a > 0, b > 0
    * b*ButtonB < prize
    */
    private IEnumerable<Solution> GetInLine(Point p1, Point p2, Point p3)
    {
        var (x1, x2, x3) = p1.X != 0 ? (p1.X, p2.X, p3.X) : (p1.Y, p2.Y, p3.Y);

        for (long b = 1 + (x3 - x2) % x1; x3 - b * x2 > 0; b += x1)
        {
            long a = (x3 - b * x2) / x1;
            yield return new Solution(a, b);
        }
    }

    private IEnumerable<long> SolveSingle(Point p1, Point p3)
    {
        var (x1, x3) = p1.X != 0 ? (p1.X, p3.X) : (p1.Y, p3.Y);

        if (x3 % x1 == 0)
        {
            yield return x3 / x1;
        }
    }

    private IEnumerable<(long, long)> SolveTwoDimensional(ClawMachine m)
    {
        var (x1, x2, x3) = (m.ButtonA.X, m.ButtonB.X, m.Prize.X);
        var (y1, y2, y3) = (m.ButtonA.Y, m.ButtonB.Y, m.Prize.Y);

        var a = (x2 * y3 - y2 * x3) / (x2 * y1 - y2 * x1);
        var b = (x3 - a * x1) / x2;

        if ((a * m.ButtonA) + (b * m.ButtonB) == m.Prize)
        {
            yield return (a, b);
        }
    }

    private bool AreParallel(Point a, Point b) => a.X * b.Y == a.Y * b.X;
}