public class WaterTile : Tile
{
    public WaterTile(int x, int y) : base(x, y)
    {
    }

    public override void Display()
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write('/');
    }

    public override bool IsWalkable()
    {
        return true;
    }
}
