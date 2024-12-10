namespace Day10;

public class Part2(int[][] map) : Part1(map)
{
    override public int CountPaths(int[][] map, (int r, int c) p)
    {
        return GetPeaks(map, p).Count();
    }
}