using Day15;

bool verbose = false;

Console.WriteLine("Part 1:");
Console.WriteLine($"Small Test: {new Board("small.txt", false, verbose).Solve()}");
Console.WriteLine($"Large Test: {new Board("test.txt", false, verbose).Solve()}");
Console.WriteLine($"Real Input: {new Board("input.txt", false, verbose).Solve()}");
Console.WriteLine();

Console.WriteLine("Part 2:");
Console.WriteLine($"Small Test: {new Board("small.txt", true, verbose).Solve()}");
Console.WriteLine($"Large Test: {new Board("test.txt", true, verbose).Solve()}");
Console.WriteLine($"Real Input: {new Board("input.txt", true, verbose).Solve()}");
