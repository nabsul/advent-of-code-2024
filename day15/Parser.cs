using System.Text;

namespace Day15;

public static class Parser
{
    public static char[,] ReadMap(StreamReader reader, bool fatMap)
    {
        string? line;
        var lines = new List<string>();
        while ((line = reader.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
        {
            var row = new StringBuilder(line);
            if (fatMap)
            {
                row = row
                    .Replace("#", "##")
                    .Replace("O", "[]")
                    .Replace(".", "..")
                    .Replace("@", "@.");
            }

            lines.Add(row.ToString());
        }

        var res = new char[lines.Count, lines[0].Length];
        for (var i = 0; i < lines.Count; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                res[i, j] = lines[i][j];
            }
        }

        return res;
    }

    public static char[] ReadMoves(StreamReader reader)
    {
        var moves = new StringBuilder();
        string? line;
        while ((line = reader.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
        {
            moves.Append(line);
        }

        return moves.ToString().ToCharArray();
    }
}