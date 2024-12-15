namespace Day15;

public static class Util
{
    public const char BotChar = '@';
    public const char WallChar = '#';
    public const char EmptyChar = '.';
    public const char BoxChar = 'O';
    public const char LeftBoxChar = '[';
    public const char RightBoxChar = ']';

    public static (int R, int C) Direction(char c) => c switch
    {
        '^' => (-1, 0),
        'v' => (1, 0),
        '<' => (0, -1),
        '>' => (0, 1),
        _ => throw new ArgumentException("Invalid direction: " + c)
    };

    public static void PrintBoard(char dir, char[,] map)
    {
        var (rows, cols) = (map.GetLength(0), map.GetLength(1));
        Console.WriteLine($"Move: {dir}");
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                Console.Write(map[i, j]);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}