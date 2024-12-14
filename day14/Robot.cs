public record Point(int X, int Y);
public class Robot(Point Position, Point Velocity)
{
    public Point Position { get; set; } = Position;
    public Point Velocity { get; set; } = Velocity;
}
