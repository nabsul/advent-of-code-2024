public class Solver(IEnumerable<ClawMachine> machines)
{
    public long Solve()
    {
        return machines.Select(Solve).Where(c => c > 0).Sum();
    }

    private long Solve(ClawMachine machine)
    {
        var results = GetPossibleAnswers(machine).ToArray();
        if (results.Length == 0)
        {
            return -1;
        }

        return results.Min(x => x.Cost);
    }

    private IEnumerable<Solution> GetPossibleAnswers(ClawMachine machine)
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
        
    }
}