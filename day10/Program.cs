using Day10;

var map = File.ReadAllLines("input.txt")
    .Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray())
    .ToArray();

var part1 = new Part1(map).Solve();
Console.WriteLine($"Part 1: {part1}");

var part2 = new Part2(map).Solve();
Console.WriteLine($"Part 2: {part2}");
