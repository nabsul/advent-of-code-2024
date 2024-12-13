using System.Text.RegularExpressions;

public class Parser
{
    private readonly Regex ButtonARegex = new(@"^Button A: X\+(\d+), Y\+(\d+)$");
    private readonly Regex ButtonBRegex = new(@"^Button B: X\+(\d+), Y\+(\d+)$");
    private readonly Regex PrizeRegex = new(@"^Prize: X=(\d+), Y=(\d+)$");

    public IEnumerable<ClawMachine> ReadAll()
    {
        var allLines = File.ReadAllLines("input.txt");

        for (int i = 0; i < allLines.Length; i += 4)
        {
            var buttonALine = allLines[i];
            var buttonBLine = allLines[i + 1];
            var prizeLine = allLines[i + 2];
            yield return ParseLines(buttonALine, buttonBLine, prizeLine);
        }
    }

    private ClawMachine ParseLines(string buttonALine, string buttonBLine, string prizeLine)
    {
        return new ClawMachine
        {
            ButtonA = Extract(buttonALine, ButtonARegex),
            ButtonB = Extract(buttonBLine, ButtonBRegex),
            Prize = Extract(prizeLine, PrizeRegex)
        };
    }

    private Coords Extract(string line, Regex regex)
    {
        var match = regex.Match(line);
        if (!match.Success)
        {
            throw new Exception($"Invalid line: {line} does not match {regex}");
        }
        
        return new Coords
        {
            X = int.Parse(match.Groups[1].Value),
            Y = int.Parse(match.Groups[2].Value)
        };
    }
}