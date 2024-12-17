namespace Day16;

public class Map
{
    private readonly char[,] _cells;
    public char Get((int, int) p) => _cells[p.Item1, p.Item2];

    public int Rows { get; }
    public int Columns { get; }

    public Map(string file)
    {
        var lines = File.ReadAllLines(file);
        Rows = lines.Length;
        Columns = lines[0].Length;
        _cells = new char[Rows, Columns];
        foreach (var (i, j) in Enumerable.Range(0, Rows).SelectMany(i => Enumerable.Range(0, Columns).Select(j => (i, j))))
        {
            _cells[i, j] = lines[i][j];
        }
    }
}