namespace Day15;

public class Board
{
    public bool Verbose { get; }
    public int Rows { get; }
    public int Cols { get; }
    public char[,] Map { get; }
    public char[] Moves { get; }
    public (int R, int C) BotPosition { get; set; }

    public Board(string file, bool fatMap, bool print)
    {
        Verbose = print;
        using var reader = new StreamReader(File.OpenRead(file));
        Map = Parser.ReadMap(reader, fatMap);
        Moves = Parser.ReadMoves(reader);
        Rows = Map.GetLength(0);
        Cols = Map.GetLength(1);
        FindBot();
    }

    public char MapGet((int, int) pos) => Map[pos.Item1, pos.Item2];
    public void MapSet((int, int) pos, char val) => Map[pos.Item1, pos.Item2] = val;
    private static (int R, int C) Add((int R, int C) a, (int R, int C) b) => (a.R + b.R, a.C + b.C);

    public long Solve()
    {
        if (Verbose) Util.PrintBoard(' ', Map);
        foreach (var move in Moves)
        {
            MoveBot(move);
            if (Verbose) Util.PrintBoard(move, Map);
        }

        return CalcScore();
    }

    public long CalcScore()
    {
        return Enumerable.Range(0, Rows)
            .SelectMany(i => Enumerable.Range(0, Cols).Select(j => (R: i, C: j)))
            .Where(p => MapGet(p) == Util.BoxChar || MapGet(p) == Util.LeftBoxChar)
            .Sum(p => (long)100 * p.R + p.C);
    }

    public void MoveBot(char d)
    {
        var dir = Util.Direction(d);
        if (!CanMove(BotPosition, dir)) return;
        Move(BotPosition, dir);
        Map[BotPosition.R, BotPosition.C] = Util.EmptyChar;
        BotPosition = (BotPosition.R + dir.R, BotPosition.C + dir.C);
    }

    private bool CanMove((int R, int C) pos, (int R, int C) dir)
    {
        var next = Add(pos, dir);

        var val = MapGet(next);
        if (val == Util.WallChar) return false;
        if (val == Util.EmptyChar) return true;

        if (dir.R == 0 || val == Util.BoxChar || val == Util.BotChar)
        {
            return CanMove(next, dir);
        }

        var shift = val == Util.LeftBoxChar ? 1 : -1;
        var next2 = Add(next, (0, shift));
        return CanMove(next, dir) && CanMove(next2, dir);
    }

    private void Move((int R, int C) pos, (int R, int C) dir)
    {
        if (MapGet(pos) == Util.EmptyChar) return;

        var next = Add(pos, dir);
        var val = MapGet(next);

        if (dir.R == 0 || val == Util.BoxChar || val == Util.BotChar || val == Util.EmptyChar)
        {
            Move(next, dir);
        }
        else
        {
            var shift = val == Util.LeftBoxChar ? 1 : -1;
            var next2 = Add(next, (0, shift));
            Move(next, dir);
            Move(next2, dir);
        }

        MapSet(next, MapGet(pos));
        MapSet(pos, Util.EmptyChar);
    }    

    public void FindBot()
    {
        BotPosition = Enumerable.Range(0, Rows)
            .SelectMany(i => Enumerable.Range(0, Cols).Select(j => (R: i, C: j)))
            .First(p => Map[p.R, p.C] == Util.BotChar);
    }
}