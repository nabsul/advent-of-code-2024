var parse = new Parser();

var machines = parse.ReadAll().ToArray();

var part1 = new Solver(machines);
Console.WriteLine($"Part 1: {part1.Solve()}");

var add = new Point(10000000000000, 10000000000000);
var m2 = machines.Select(m => new ClawMachine
{
    ButtonA = m.ButtonA,
    ButtonB = m.ButtonB,
    Prize = m.Prize + add,
}).ToArray();

var part2 = new Solver(m2);
Console.WriteLine($"Part 2: {part2.Solve()}");
