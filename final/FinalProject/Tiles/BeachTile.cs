public class BeachTile : Tile
{
    public BeachTile(int x, int y) : base(x, y)
    {
    }

    public override void Display()
    {
        Console.BackgroundColor = ConsoleColor.Yellow;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write('_');
    }

    public override bool IsWalkable()
    {
        return true;
    }
}
