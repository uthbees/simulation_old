public class GroundTile: Tile
{
    public GroundTile(int x, int y) : base(x, y)
    {
    }
    
    public override void Display()
    {
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write('"');
    }

    public override bool IsWalkable()
    {
        return true;
    }
}
