public class WaterTile : Tile
{
    public override void Display()
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write('`');
    }

    public override bool IsWalkable()
    {
        return true;
    }
}
