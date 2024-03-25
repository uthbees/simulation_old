public class MountainTile : Tile
{
    public MountainTile(int x, int y) : base(x, y)
    {
    }

    public override void Display()
    {
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write('^');
    }

    public override bool IsWalkable()
    {
        return true;
    }
}
