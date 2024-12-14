var robots = Parser.Read("input.txt").ToArray();
Console.WriteLine($"Part 1: {Part1.Solve(robots, 100, (101, 103))}");

if (!Directory.Exists("img"))
{
    Directory.CreateDirectory("img");
}

int start = int.Parse(args[0]);
int incr = int.Parse(args[1]);
int end = int.Parse(args[2]);
Console.WriteLine($"Part 2: {Part2.Solve(robots, start, incr, end, (101, 103))}");
