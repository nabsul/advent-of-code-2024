using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class Parser
{
    
    [GeneratedRegex((@"^p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)$"))]
    private static partial Regex LineRegex();

    public static IEnumerable<Robot> Read(string input)
    {
        var regex = LineRegex();
        using var reader = new StreamReader(File.OpenRead(input));
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var match = regex.Match(line);
            if (!match.Success)
            {
                throw new Exception($"Failed to parse line: {line}");
            }
            yield return new Robot(
                new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))
            );
        }
    }
}
