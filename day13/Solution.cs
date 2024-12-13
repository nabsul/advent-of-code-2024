public record Solution(long NumA, long NumB)
{
    public long Cost => 3 * NumA + NumB;
}
